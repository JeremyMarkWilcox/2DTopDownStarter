extends Node
var checks := 0
var failed := false
var motion_events := 0

func check(ok: bool, message: String) -> void:
	if ok:
		checks += 1
		print("PASS: " + message)
	else:
		failed = true
		push_error("FAIL: " + message)

func frames(n: int = 4) -> void:
	for i in n:
		await get_tree().physics_frame
	await get_tree().process_frame

func _ready() -> void:
	if not "--smoke-test" in OS.get_cmdline_user_args():
		get_tree().quit(1)
		return
	process_mode = Node.PROCESS_MODE_ALWAYS
	get_tree().current_scene = null
	get_tree().create_timer(25).timeout.connect(func(): get_tree().quit(1))
	run.call_deferred()

func run() -> void:
	SaveManager.DeleteSave(1)
	SaveManager.NewGame(1)
	SceneFlowManager.FadeDuration = 0.01
	SceneFlowManager.ReturnToMainMenu()
	await SceneFlowManager.TransitionFinished
	await frames()
	MenuManager.ActiveMenu.StartButton.pressed.emit()
	await SceneFlowManager.TransitionFinished
	await frames()
	var level = get_tree().current_scene
	check(level.scene_file_path == "res://Game/TopDown/OverheadDemo.tscn", "New Game starts the overhead prototype")
	SaveManager.NewGame(96)
	var player = level.Player
	var spawn: Vector2 = player.global_position
	player.MotionChanged.connect(func(_facing, _moving): motion_events += 1)
	check(ProjectSettings.get_setting("display/window/stretch/mode") == "viewport" and ProjectSettings.get_setting("display/window/stretch/scale_mode") == "integer", "pixel viewport and integer scaling applied")
	if "--capture-preview" in OS.get_cmdline_user_args():
		await RenderingServer.frame_post_draw
		get_viewport().get_texture().get_image().save_png("res://.godot/topdown-preview.png")
	Input.action_press("move_right")
	await frames(8)
	check(player.global_position.x > spawn.x + 5, "horizontal movement")
	check(is_equal_approx(player.velocity.length(), player.Speed), "straight speed")
	Input.action_press("move_down")
	await frames(4)
	check(is_equal_approx(player.velocity.length(), player.Speed), "diagonal speed remains normalized")
	Input.action_release("move_right")
	Input.action_release("move_down")
	await frames()
	check(player.velocity == Vector2.ZERO and motion_events >= 3, "idle and movement animation hook")
	var idle: Vector2 = player.global_position
	await frames(10)
	check(player.global_position.is_equal_approx(idle), "no gravity in overhead movement")
	player.global_position = Vector2(360, 240)
	Input.action_press("move_right")
	await frames(35)
	Input.action_release("move_right")
	check(player.global_position.x < 380, "solid obstacle blocks movement")
	player.global_position = spawn
	MenuManager.OpenMenu(level.PauseMenu)
	Input.action_press("move_right")
	await frames(8)
	check(player.global_position.is_equal_approx(spawn), "pause blocks polled movement")
	check(not level.SaveProgress(), "pause blocks gameplay save")
	MenuManager.CloseMenu()
	Input.action_release("move_right")
	await frames()
	check(not get_tree().paused, "resume restores gameplay")
	player.global_position = Vector2(176, 302)
	await frames()
	check(level.FindInteractable() != null, "nearby sign detected")
	level.TryInteract()
	check(level.StatusLabel.text.contains("Blue"), "sign interaction")
	player.global_position = Vector2(304, 304)
	await frames()
	level.TryInteract()
	check(SaveManager.GetCheckpointId() == "trail", "checkpoint activates and persists")
	player.global_position = Vector2(584, 360)
	await frames(8)
	check(player.global_position.distance_to(Vector2(304, 304)) < 1, "hazard respawns at checkpoint")
	player.global_position = Vector2(720, 300)
	await frames()
	check(level.SaveProgress(), "manual position save")
	SceneFlowManager.RestartCurrentScene()
	await SceneFlowManager.TransitionFinished
	await frames()
	level = get_tree().current_scene
	check(level.Player.global_position.distance_to(Vector2(720, 300)) < 1, "restart restores saved position")
	level.Respawn()
	check(level.Player.global_position.distance_to(Vector2(304, 304)) < 1, "checkpoint survives scene reload")
	level.Player.global_position = Vector2(800, 510)
	await frames()
	level.TryInteract()
	await SceneFlowManager.TransitionFinished
	await frames()
	check(MenuManager.ActiveMenu.ContinueButton.visible, "exit saves and offers Continue")
	MenuManager.ActiveMenu.ContinueButton.pressed.emit()
	await SceneFlowManager.TransitionFinished
	await frames()
	check(get_tree().current_scene.Player.global_position.distance_to(Vector2(800, 510)) < 1, "Continue restores prototype position")
	SaveManager.DeleteSave(96)
	print("TOP DOWN %s (%d checks)" % ["FAIL" if failed else "PASS", checks])
	get_tree().quit(1 if failed else 0)
