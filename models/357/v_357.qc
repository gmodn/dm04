// Created by Crowbar 0.71

$modelname "weapons/v_357.mdl"

$bodygroup "studio"
{
	studio "Python_357_reference.smd"
}


$surfaceprop "default"

$contents "solid"

$illumposition -3.224 -12.453 -9.997

$cdmaterials "models\Weapons\V_hand\"
$cdmaterials "models\Weapons\V_357\"

$attachment "muzzle" "muzzle" 0 0 0 rotate -90 90 0

$cbox 0 0 0 0 0 0

$bbox -5.078 -12.171 -17.396 29.984 5.724 -2.599

$definebone "Base" "" 0 -0.878417 54.32534 0 0 89.999982 0 0 0 0 0 0
$definebone "Bip01_L_UpperArm" "Base" 7.723854 4.07111 -1.956161 1.679511 -86.402549 -80.679847 0 0 0 0 0 0
$definebone "Bip01_L_Forearm" "Bip01_L_UpperArm" 11.692556 0 0 0 -42.93127 0 0 0 0 0 0 0
$definebone "Bip01_L_Hand" "Bip01_L_Forearm" 11.481673 0.000001 -0.000032 -1.608522 2.542788 90.13606 0 0 0 0 0 0
$definebone "Bip01_L_Finger4" "Bip01_L_Hand" 3.859703 -0.142405 -1.191974 11.405757 -46.283698 -26.0878 0 0 0 0 0 0
$definebone "Bip01_L_Finger41" "Bip01_L_Finger4" 1.312563 0.000001 -0.000001 0 -24.951507 0 0 0 0 0 0 0
$definebone "Bip01_L_Finger42" "Bip01_L_Finger41" 0.729364 0.000002 0 0 -13.762849 0 0 0 0 0 0 0
$definebone "Bip01_L_Finger3" "Bip01_L_Hand" 3.942325 0.046771 -0.431442 5.388669 -37.796426 -8.028456 0 0 0 0 0 0
$definebone "Bip01_L_Finger31" "Bip01_L_Finger3" 1.539092 0 0 0 -20.09311 0 0 0 0 0 0 0
$definebone "Bip01_L_Finger32" "Bip01_L_Finger31" 1.196322 0 0 0.000001 -33.315491 0 0 0 0 0 0 0
$definebone "Bip01_L_Finger2" "Bip01_L_Hand" 3.883687 -0.046771 0.431444 1.36215 -32.342496 4.890194 0 0 0 0 0 0
$definebone "Bip01_L_Finger21" "Bip01_L_Finger2" 1.719576 0 -0.000001 0.000001 -23.460216 -0.000002 0 0 0 0 0 0
$definebone "Bip01_L_Finger22" "Bip01_L_Finger21" 1.209183 0 -0.000001 0.000001 -14.6986 -0.000002 0 0 0 0 0 0
$definebone "Bip01_L_Finger1" "Bip01_L_Hand" 3.859861 -0.137948 1.332465 -3.031803 -28.017404 19.855449 0 0 0 0 0 0
$definebone "Bip01_L_Finger11" "Bip01_L_Finger1" 1.71943 0.000001 0 0.000001 -23.931817 -0.000003 0 0 0 0 0 0
$definebone "Bip01_L_Finger12" "Bip01_L_Finger11" 1.099666 -0.000001 0 0 -16.932162 -0.000003 0 0 0 0 0 0
$definebone "Bip01_L_Finger0" "Bip01_L_Hand" 0.806045 -0.348922 1.321259 -36.272584 -53.021454 -66.713142 0 0 0 0 0 0
$definebone "Bip01_L_Finger01" "Bip01_L_Finger0" 1.789784 0 0 -0.000003 12.177989 0.000002 0 0 0 0 0 0
$definebone "Bip01_L_Finger02" "Bip01_L_Finger01" 1.206997 -0.000003 0 -0.000004 16.301737 0.000001 0 0 0 0 0 0
$definebone "Arm" "Base" -6.211924 -0.713153 8.058478 0 0 0 0 0 0 0 0 0
$definebone "Hand" "Arm" 0.46347 5.506761 13.08023 0 0 0 0 0 0 0 0 0
$definebone "Python" "Hand" 0.09978 3.177673 4.325963 0.207296 -1.050805 -0.515204 0 0 0 0 0 0
$definebone "muzzle" "Python" -0.000131 0.765269 8.032604 0 0 0 0 0 0 0 0 0
$definebone "Cylinder_release" "Python" 0.025546 -1.273521 1.753298 0 0 0 0 0 0 0 0 0
$definebone "Cylinder" "Cylinder_release" 0.001032 1.440439 -1.526784 0 0 0 0 0 0 0 0 0
$definebone "Bullet1" "Cylinder" 0.004413 0.696493 -1.430599 0 0 0 0 0 0 0 0 0
$definebone "Bullet2" "Cylinder" 0.58066 0.34901 -1.430598 0 0 0 0 0 0 0 0 0
$definebone "Bullet3" "Cylinder" 0.583443 -0.305906 -1.430598 0 0 0 0 0 0 0 0 0
$definebone "Bullet4" "Cylinder" 0.005317 -0.648725 -1.430598 0 0 0 0 0 0 0 0 0
$definebone "Bullet5" "Cylinder" -0.574618 -0.303122 -1.430599 0 0 0 0 0 0 0 0 0
$definebone "Bullet6" "Cylinder" -0.573714 0.348107 -1.430599 0 0 0 0 0 0 0 0 0


$sequence "idle01" {
	"v_357_anims\idle01.smd"
	activity "ACT_VM_IDLE" 1
	fadein 0.2
	fadeout 0.2
	node "Ready"
	fps 30
	loop
}

$sequence "fire" {
	"v_357_anims\fire.smd"
	activity "ACT_VM_PRIMARYATTACK" 1
	{ event AE_MUZZLEFLASH 0 "357 muzzle" }
	fadein 0.2
	fadeout 0.2
	node "Fire"
	snap
	fps 30
}

$sequence "draw" {
	"v_357_anims\draw.smd"
	activity "ACT_VM_DRAW" 1
	fadein 0.2
	fadeout 0.2
	node "Ready"
	snap
	fps 30
}

$sequence "reload" {
	"v_357_anims\reload.smd"
	activity "ACT_VM_RELOAD" 1
	{ event AE_CL_PLAYSOUND 28 "Weapon_357.OpenLoader" }
	{ event AE_CL_PLAYSOUND 39 "Weapon_357.RemoveLoader" }
	{ event 3015 55 "" }
	{ event AE_CL_PLAYSOUND 67 "Weapon_357.ReplaceLoader" }
	{ event AE_CL_PLAYSOUND 92 "Weapon_357.Spin" }
	fadein 0.2
	fadeout 0.2
	node "Ready"
	fps 30
}

$sequence "holster" {
	"v_357_anims\holster.smd"
	activity "ACT_VM_HOLSTER" 1
	fadein 0.2
	fadeout 0.2
	node "Ready"
	fps 30
}

$sequence "idletolow" {
	"v_357_anims\idletolow.smd"
	activity "ACT_VM_LOWERED_TO_IDLE" 1
	fadein 0.2
	fadeout 0.2
	transition "Ready" "Low"
	fps 30
}

$sequence "lowtoidle" {
	"v_357_anims\lowtoidle.smd"
	activity "ACT_VM_IDLE_TO_LOWERED" 1
	fadein 0.2
	fadeout 0.2
	transition "Low" "Ready"
	fps 30
}

$sequence "lowidle" {
	"v_357_anims\lowidle.smd"
	activity "ACT_VM_IDLE_LOWERED" 1
	fadein 0.2
	fadeout 0.2
	node "Low"
	fps 30
	loop
}
