// Created by Crowbar 0.71

$modelname "Weapons/v_RPG.mdl"

$bodygroup "studio"
{
	studio "RPG_reference.smd"
}


$surfaceprop "default"

$contents "solid"

$illumposition -16.708 -36.922 -8.533

$cdmaterials "Models\Weapons\v_Rocket_launcher\"
$cdmaterials "models\Weapons\V_hand\"

$attachment "0" "muzzle" 0 0 0 rotate 0 0 0
$attachment "laser" "muzzle" -9 -5 0.5 rotate 0 0 0
$attachment "laser_end" "muzzle" 100 -5 0.5 rotate 0 0 0

$cbox 0 0 0 0 0 0

$bbox 19.811 -26.381 -19.319 54.034 -7.035 2.252

$definebone "base" "" -16.057949 -24.995756 64.720612 0 0 89.999982 0 0 0 0 0 0
$definebone "muzzle" "base" -0.454765 0.279387 29.004244 -89.999982 0 0 0 0 0 0 0 0
$definebone "arm" "base" 1.26345 -8.255469 7.260771 0 0 0 0 0 0 0 0 0


$sequence "idle1" {
	"v_rpg_anims\idle1.smd"
	activity "ACT_VM_IDLE" 1
	fadein 0.2
	fadeout 0.2
	node "Idle"
	fps 30
	loop
}

$sequence "draw" {
	"v_rpg_anims\draw.smd"
	activity "ACT_VM_DRAW" 1
	fadein 0.2
	fadeout 0.2
	node "Idle"
	snap
	fps 30
}

$sequence "fire" {
	"v_rpg_anims\fire.smd"
	activity "ACT_VM_PRIMARYATTACK" 1
	{ event 5001 0 "" }
	fadein 0.2
	fadeout 0.2
	node "Fire"
	snap
	fps 30
}

$sequence "reload" {
	"v_rpg_anims\reload.smd"
	activity "ACT_VM_RELOAD" 1
	fadein 0.2
	fadeout 0.2
	node "Idle"
	fps 30
}

$sequence "up_to_down" {
	"v_rpg_anims\up_to_down.smd"
	activity "ACT_VM_IDLE_TO_LOWERED" 1
	fadein 0.2
	fadeout 0.2
	transition "Idle" "Low"
	fps 30
}

$sequence "down_to_up" {
	"v_rpg_anims\down_to_up.smd"
	activity "ACT_VM_LOWERED_TO_IDLE" 1
	fadein 0.2
	fadeout 0.2
	transition "Low" "Idle"
	fps 30
}

$sequence "down_idle" {
	"v_rpg_anims\down_idle.smd"
	activity "ACT_VM_IDLE_LOWERED" 1
	fadein 0.2
	fadeout 0.2
	node "Low"
	fps 30
	loop
}
