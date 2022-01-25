// Created by Crowbar 0.71

$modelname "weapons/v_IRifle.mdl"

$bodygroup "studio"
{
	studio "IRifle_reference.smd"
}


$surfaceprop "default"

$contents "solid"

$illumposition -6.084 -20.728 -7.257

$cdmaterials "models\Weapons\V_hand\"
$cdmaterials "models\Weapons\V_irifle\"

$attachment "muzzle" "Base" 0 0 20 rotate -90 0 0

$cbox 0 0 0 0 0 0

$bbox 3.083 -12.016 -14.641 38.372 -0.152 0.126

$definebone "Base" "" -5.846632 -11.638401 61.824333 0 0 89.999982 0 0 0 0 0 0
$definebone "Reload" "Base" 1.526086 -2.764851 14.082601 0 0 0 0 0 0 0 0 0
$definebone "Reload1" "Reload" 0.832381 1.842239 -0.000374 0 0 0 0 0 0 0 0 0
$definebone "Shell1" "Base" -0.05046 -0.056505 18.287695 0 0 0 0 0 0 0 0 0
$definebone "Bolt1" "Base" -0.112315 0.055738 8.441935 0 0 0 0 0 0 0 0 0
$definebone "Bolt2" "Bolt1" 0.487353 -0.167217 2.950581 0 0 0 0 0 0 0 0 0
$definebone "Shell2" "Base" 1.617459 -1.028969 14.611864 0 0 0 0 0 0 0 0 0
$definebone "Vent" "Base" 0.790197 0.067793 19.411802 0 0 0 0 0 0 0 0 0
$definebone "Claw2" "Base" 0.599579 0.805453 17.492916 0 -38.249978 -30.000014 0 0 0 0 0 0
$definebone "Claw1" "Base" 1.023674 -0.207506 17.423468 30.000018 -6.750017 0 0 0 0 0 0 0


$poseparameter "VentPoses" 0 1 loop 0

$animation "a_vent_open_corrective_animation" "v_irifle_anims\a_vent_open_corrective_animation.smd" {
}

$animation "a_vent_closed_corrective_animation" "v_irifle_anims\a_vent_closed_corrective_animation.smd" {
}

$animation "a_vent_neutral" "v_irifle_anims\a_vent_neutral.smd" {
	fps 30
}

$animation "a_vent_open" "v_irifle_anims\a_vent_open.smd" {
	fps 30
	// Crowbar writes this subtract option to prevent jigglebone and poseparameter problems when delta sequences are recompiled.
	subtract "a_vent_open_corrective_animation" 0
}

$animation "a_vent_closed" "v_irifle_anims\a_vent_closed.smd" {
	fps 30
	// Crowbar writes this subtract option to prevent jigglebone and poseparameter problems when delta sequences are recompiled.
	subtract "a_vent_closed_corrective_animation" 0
}

$sequence "IR_idle" {
	"v_irifle_anims\IR_idle.smd"
	activity "ACT_VM_IDLE" 1
	fadein 0.2
	fadeout 0.2
	node "Ready"
	fps 30
	loop
}

$sequence "IR_fire" {
	"v_irifle_anims\IR_fire.smd"
	activity "ACT_VM_PRIMARYATTACK" 1
	{ event AE_MUZZLEFLASH 0 "COMBINE MUZZLE" }
	fadein 0.2
	fadeout 0.2
	node "Fire"
	snap
	fps 30
}

$sequence "fire2" {
	"v_irifle_anims\fire2.smd"
	activity "ACT_VM_RECOIL1" 1
	{ event AE_MUZZLEFLASH 0 "COMBINE MUZZLE" }
	fadein 0.2
	fadeout 0.2
	node "Fire"
	snap
	fps 30
}

$sequence "fire3" {
	"v_irifle_anims\fire3.smd"
	activity "ACT_VM_RECOIL2" 1
	{ event AE_MUZZLEFLASH 0 "COMBINE MUZZLE" }
	fadein 0.2
	fadeout 0.2
	node "Fire"
	snap
	fps 30
}

$sequence "fire4" {
	"v_irifle_anims\fire4.smd"
	activity "ACT_VM_RECOIL3" 1
	{ event AE_MUZZLEFLASH 0 "COMBINE MUZZLE" }
	fadein 0.2
	fadeout 0.2
	node "Fire"
	snap
	fps 30
}

$sequence "IR_fire2" {
	"v_irifle_anims\IR_fire2.smd"
	activity "ACT_VM_SECONDARYATTACK" 1
	{ event AE_MUZZLEFLASH 0 "COMBINE MUZZLE" }
	fadein 0.2
	fadeout 0.2
	snap
	fps 30
}

$sequence "IR_reload" {
	"v_irifle_anims\IR_reload.smd"
	activity "ACT_VM_RELOAD" 1
	{ event AE_CL_PLAYSOUND 1 "Weapon_AR2.Reload_Rotate" }
	{ event AE_CL_PLAYSOUND 19 "Weapon_AR2.Reload_Push" }
	fadein 0.2
	fadeout 0.2
	node "Ready"
	fps 30
}

$sequence "IR_draw" {
	"v_irifle_anims\IR_draw.smd"
	activity "ACT_VM_DRAW" 1
	fadein 0.2
	fadeout 0.2
	node "Ready"
	snap
	fps 30
}

$sequence "IR_holster" {
	"v_irifle_anims\IR_holster.smd"
	activity "ACT_VM_HOLSTER" 1
	fadein 0.2
	fadeout 0.2
	node "Ready"
	fps 30
}

$sequence "idletolow" {
	"v_irifle_anims\idletolow.smd"
	activity "ACT_VM_LOWERED_TO_IDLE" 1
	fadein 0.2
	fadeout 0.2
	transition "Ready" "Low"
	fps 30
}

$sequence "lowtoidle" {
	"v_irifle_anims\lowtoidle.smd"
	activity "ACT_VM_IDLE_TO_LOWERED" 1
	fadein 0.2
	fadeout 0.2
	transition "Low" "Ready"
	fps 30
}

$sequence "lowidle" {
	"v_irifle_anims\lowidle.smd"
	activity "ACT_VM_IDLE_LOWERED" 1
	fadein 0.2
	fadeout 0.2
	node "Low"
	fps 30
	loop
}

$sequence "shake" {
	"v_irifle_anims\shake.smd"
	activity "ACT_VM_FIDGET" 1
	fadein 0.2
	fadeout 0.2
	node "Fire"
	fps 30
	loop
}

$sequence "VentPoses" {
	"a_vent_open"
	"a_vent_closed"
	autoplay
	blend "VentPoses" 1 0
	blendwidth 2
	delta
	fadein 0.2
	fadeout 0.2
}
