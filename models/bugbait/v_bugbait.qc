// Created by Crowbar 0.71

$modelname "weapons/V_bugbait.mdl"

$bodygroup "studio"
{
	studio "Bugbait_reference.smd"
}


$surfaceprop "default"

$contents "solid"

$illumposition -8.144 -14.448 -16.05

$cdmaterials "models\Weapons\V_hand\"
$cdmaterials "models\Weapons\V_bugbait\"

$attachment "0" "ValveBiped.Bip01_R_Hand" 4 -4 0 rotate 0 0 0

$cbox 0 0 0 0 0 0

$bbox -1.52 -16.361 -29.433 30.417 0.073 -2.667

$definebone "ValveBiped.Bip01" "" -0.000005 -4.614097 38.566917 0 0 89.999982 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine" "ValveBiped.Bip01" 0.000005 3.345127 -2.981901 4.944225 89.999982 89.999982 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine1" "ValveBiped.Bip01_Spine" 4.01833 0 0 0 -1.675443 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine2" "ValveBiped.Bip01_Spine1" 3.518568 0 0 0 5.74883 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine4" "ValveBiped.Bip01_Spine2" 8.94264 0 0 0 11.120881 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Clavicle" "ValveBiped.Bip01_Spine4" 2.033354 1.000771 -1.93761 73.712037 168.618779 -95.596496 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_UpperArm" "ValveBiped.Bip01_R_Clavicle" 6.028143 0.000002 0 77.991703 -32.315394 103.017353 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Forearm" "ValveBiped.Bip01_R_UpperArm" 11.692551 -0.000002 -0.000001 0.000001 -34.576344 0.000001 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Hand" "ValveBiped.Bip01_R_Forearm" 11.481699 -0.000001 -0.000001 -8.201205 20.638511 -14.704103 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger4" "ValveBiped.Bip01_R_Hand" 3.859671 -0.132567 1.193108 -28.381979 -56.331032 21.753717 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger41" "ValveBiped.Bip01_R_Finger4" 1.312561 0 0.000002 0.000002 -26.321053 0.000001 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger42" "ValveBiped.Bip01_R_Finger41" 0.729363 -0.000001 -0.000001 0.000002 -15.08695 0.000001 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger3" "ValveBiped.Bip01_R_Hand" 3.942294 0.050331 0.431045 -20.334843 -33.642476 1.247503 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger31" "ValveBiped.Bip01_R_Finger3" 1.539097 -0.000001 0 0.000003 -22.460114 0.000001 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger32" "ValveBiped.Bip01_R_Finger31" 1.196324 0.000001 0 0.000004 -36.745499 -0.000001 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger2" "ValveBiped.Bip01_R_Hand" 3.883657 -0.050326 -0.431041 -3.447083 -34.19527 -5.53122 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger21" "ValveBiped.Bip01_R_Finger2" 1.719577 -0.000001 -0.000001 0.000003 -25.608925 -0.000001 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger22" "ValveBiped.Bip01_R_Finger21" 1.209183 0 -0.000001 0.000001 -16.741424 -0.000002 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger1" "ValveBiped.Bip01_R_Hand" 3.859828 -0.148933 -1.33128 12.775988 -52.654359 -12.732782 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger11" "ValveBiped.Bip01_R_Finger1" 1.719422 0.000001 0 0.000002 -26.996565 -0.000001 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger12" "ValveBiped.Bip01_R_Finger11" 1.099663 0 0.000001 0.000001 -19.847715 -0.000001 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger0" "ValveBiped.Bip01_R_Hand" 0.806016 -0.359831 -1.318332 12.297453 -70.525665 67.509897 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger01" "ValveBiped.Bip01_R_Finger0" 1.789784 -0.000001 0 0 5.239811 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Finger02" "ValveBiped.Bip01_R_Finger01" 1.206998 0.000001 0.000001 0.000001 9.342758 0.000001 0 0 0 0 0 0
$definebone "ValveBiped.cube3" "ValveBiped.Bip01_R_Hand" 3.74758 -3.455018 -0.349419 -35.544755 -72.609856 -89.621945 0 0 0 0 0 0
$definebone "ValveBiped.cube2" "ValveBiped.cube3" 2.602257 0.757129 -1.240374 28.202877 16.693414 3.689219 0 0 0 0 0 0
$definebone "ValveBiped.cube1" "ValveBiped.cube3" 2.602257 0.757129 -1.240374 28.202877 16.693414 3.689219 0 0 0 0 0 0
$definebone "ValveBiped.cube" "ValveBiped.cube3" 2.602257 0.757129 -1.240374 28.202877 16.693414 3.689219 0 0 0 0 0 0


$sequence "drawback" {
	"v_bugbait_anims\drawback.smd"
	activity "ACT_VM_HAULBACK" 1
	{ event 3900 4 "" }
	fadein 0.2
	fadeout 0.2
	fps 30
}

$sequence "throw" {
	"v_bugbait_anims\throw.smd"
	activity "ACT_VM_THROW" 1
	{ event 3005 1 "" }
	fadein 0.2
	fadeout 0.2
	fps 30
}

$sequence "idle01" {
	"v_bugbait_anims\idle01.smd"
	activity "ACT_VM_IDLE" 1
	fadein 0.2
	fadeout 0.2
	fps 30
	loop
}

$sequence "draw" {
	"v_bugbait_anims\draw.smd"
	activity "ACT_VM_DRAW" 1
	fadein 0.2
	fadeout 0.2
	snap
	fps 30
}

$sequence "holster" {
	"v_bugbait_anims\holster.smd"
	activity "ACT_VM_HOLSTER" 1
	fadein 0.2
	fadeout 0.2
	fps 30
}

$sequence "squeeze" {
	"v_bugbait_anims\squeeze.smd"
	activity "ACT_VM_SECONDARYATTACK" 1
	fadein 0.2
	fadeout 0.2
	fps 30
}
