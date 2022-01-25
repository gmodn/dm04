// Created by Crowbar 0.71

$modelname "weapons/v_smg1.mdl"

$bodygroup "studio"
{
	studio "Smg1_reference.smd"
}


$surfaceprop "default"

$contents "solid"

$illumposition -1.154 -17.271 -16.696

$cdmaterials "models\Weapons\V_hand\"
$cdmaterials "models\Weapons\V_smg1\"

$attachment "muzzle" "ValveBiped.muzzle" 0 0 0 rotate 0 0 0
$attachment "1" "ValveBiped.eject" 0 0 0 rotate 0 0 0

$cbox 0 0 0 0 0 0

$bbox 0 -10.294 -33.392 34.542 7.986 0

$definebone "ValveBiped.Bip01" "" -0.000005 -9.614098 38.566917 0 0 89.999982 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine" "ValveBiped.Bip01" 0.000005 3.345129 -2.981901 4.944225 89.999982 89.999982 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine1" "ValveBiped.Bip01_Spine" 4.018334 0 0 0 -1.675443 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine2" "ValveBiped.Bip01_Spine1" 3.518569 0 0 0 5.74883 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine4" "ValveBiped.Bip01_Spine2" 8.942642 0 0 0 11.120881 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Clavicle" "ValveBiped.Bip01_Spine4" 2.033345 1.000767 1.93766 -73.711859 168.619271 91.480542 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_UpperArm" "ValveBiped.Bip01_L_Clavicle" 6.028141 0 0 -75.092596 -71.833496 -91.620362 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Forearm" "ValveBiped.Bip01_L_UpperArm" 11.692554 -0.000004 0 0 -20.800142 0.000002 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Hand" "ValveBiped.Bip01_L_Forearm" 11.48167 0.000004 -0.000032 -1.608522 2.542789 90.13606 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger4" "ValveBiped.Bip01_L_Hand" 3.859703 -0.142405 -1.191975 7.699636 -55.291573 -27.317541 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger41" "ValveBiped.Bip01_L_Finger4" 1.312562 0.000001 0.000001 -0.000001 -16.005459 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger42" "ValveBiped.Bip01_L_Finger41" 0.729363 -0.000001 0.000001 -0.000001 -5.557285 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger3" "ValveBiped.Bip01_L_Hand" 3.942326 0.046771 -0.431441 5.010287 -42.489401 -8.31173 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger31" "ValveBiped.Bip01_L_Finger3" 1.539092 -0.000001 -0.000002 -0.000001 -17.727829 -0.000002 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger32" "ValveBiped.Bip01_L_Finger31" 1.196321 -0.000001 0 -0.000002 -29.870179 -0.000001 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger2" "ValveBiped.Bip01_L_Hand" 3.883687 -0.046771 0.431444 1.250767 -33.682296 4.92182 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger21" "ValveBiped.Bip01_L_Finger2" 1.719574 -0.000001 -0.000003 0.316043 -20.997697 -0.000002 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger22" "ValveBiped.Bip01_L_Finger21" 1.209182 0 -0.000001 0.1767 -12.398743 -0.000002 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger1" "ValveBiped.Bip01_L_Hand" 3.859861 -0.137948 1.332465 -2.449394 -30.465198 20.30843 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger11" "ValveBiped.Bip01_L_Finger1" 1.719428 -0.000003 0 0.233423 -20.798884 -0.000002 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger12" "ValveBiped.Bip01_L_Finger11" 1.099666 0 -0.000002 0.14874 -13.99908 -0.000002 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger0" "ValveBiped.Bip01_L_Hand" 0.806046 -0.348922 1.321258 -35.907032 -52.827282 -66.827513 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger01" "ValveBiped.Bip01_L_Finger0" 1.789784 0.000001 -0.000001 0.000004 10.839159 0.000001 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_L_Finger02" "ValveBiped.Bip01_L_Finger01" 1.206997 0 -0.000002 0.000004 17.539441 0.000002 0 0 0 0 0 0
$definebone "ValveBiped.base" "ValveBiped.Bip01_Spine2" 11.636151 20.498386 -8.400481 -89.999982 -89.129201 0 0 0 0 0 0 0
$definebone "ValveBiped.handle" "ValveBiped.base" 0.087378 -1.651554 7.158632 0 0 0 0 0 0 0 0 0
$definebone "ValveBiped.clip" "ValveBiped.base" 0.037136 -5.104296 -1.70817 0 0 0 0 0 0 0 0 0
$definebone "ValveBiped.eject" "ValveBiped.base" -0.488878 2.223237 -1.367224 -0.000019 180.000005 -180.000005 0 0 0 0 0 0
$definebone "ValveBiped.muzzle" "ValveBiped.base" 0.018969 0.548841 13.887943 -89.999982 0 0 0 0 0 0 0 0
$definebone "ValveBiped" "" 0 0 0 0 0 89.999982 0 0 0 0 0 0


$sequence "idle01" {
	"v_smg1_anims\idle01.smd"
	activity "ACT_VM_IDLE" 1
	fadein 0.2
	fadeout 0.2
	node "Ready"
	fps 30
	loop
}

$sequence "fire01" {
	"v_smg1_anims\fire01.smd"
	activity "ACT_VM_PRIMARYATTACK" 1
	{ event AE_MUZZLEFLASH 0 "SMG1 MUZZLE" }
	{ event 6001 0 "0" }
	fadein 0.2
	fadeout 0.2
	node "Fire"
	snap
	fps 30
}

$sequence "fire02" {
	"v_smg1_anims\fire02.smd"
	activity "ACT_VM_RECOIL1" 1
	{ event AE_MUZZLEFLASH 0 "SMG1 MUZZLE" }
	{ event 6001 0 "0" }
	fadein 0.2
	fadeout 0.2
	node "Fire"
	snap
	fps 30
}

$sequence "fire03" {
	"v_smg1_anims\fire03.smd"
	activity "ACT_VM_RECOIL2" 1
	{ event AE_MUZZLEFLASH 0 "SMG1 MUZZLE" }
	{ event 6001 0 "0" }
	fadein 0.2
	fadeout 0.2
	node "Fire"
	snap
	fps 30
}

$sequence "fire04" {
	"v_smg1_anims\fire04.smd"
	activity "ACT_VM_RECOIL3" 1
	{ event AE_MUZZLEFLASH 0 "SMG1 MUZZLE" }
	{ event 6001 0 "0" }
	fadein 0.2
	fadeout 0.2
	node "Fire"
	snap
	fps 30
}

$sequence "altfire" {
	"v_smg1_anims\altfire.smd"
	activity "ACT_VM_SECONDARYATTACK" 1
	fadein 0.2
	fadeout 0.2
	node "Fire"
	snap
	fps 30
}

$sequence "draw" {
	"v_smg1_anims\draw.smd"
	activity "ACT_VM_DRAW" 1
	fadein 0.2
	fadeout 0.2
	node "Ready"
	snap
	fps 30
}

$sequence "reload" {
	"v_smg1_anims\reload.smd"
	activity "ACT_VM_RELOAD" 1
	fadein 0.2
	fadeout 0.2
	node "Ready"
	fps 30
}

$sequence "dryfire" {
	"v_smg1_anims\dryfire.smd"
	activity "ACT_VM_DRYFIRE" 1
	fadein 0.2
	fadeout 0.2
	node "Fire"
	snap
	fps 30
}

$sequence "idletolow" {
	"v_smg1_anims\idletolow.smd"
	activity "ACT_VM_LOWERED_TO_IDLE" 1
	fadein 0.2
	fadeout 0.2
	transition "Ready" "Low"
	fps 30
}

$sequence "lowtoidle" {
	"v_smg1_anims\lowtoidle.smd"
	activity "ACT_VM_IDLE_TO_LOWERED" 1
	fadein 0.2
	fadeout 0.2
	transition "Low" "Ready"
	fps 30
}

$sequence "lowidle" {
	"v_smg1_anims\lowidle.smd"
	activity "ACT_VM_IDLE_LOWERED" 1
	fadein 0.2
	fadeout 0.2
	node "Low"
	fps 30
	loop
}
