// Created by Crowbar 0.71

$modelname "Weapons/v_crowbar.mdl"

$bodygroup "studio"
{
	studio "crowbar_reference.smd"
}


$surfaceprop "default"

$contents "solid"

$illumposition -8.644 -25.37 -15.379

$cdmaterials "models\Weapons\V_hand\"
$cdmaterials "models\Weapons\V_crowbar\"

$attachment "0" "ValveBiped.Tip" 0 0 0 rotate 0 0 0

$cbox 0 0 0 0 0 0

$bbox 7.416 -17.287 -33.063 43.324 0 2.306

$definebone "ValveBiped.Bip01" "" -0.000005 -28.696043 38.566917 0 0 89.999982 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine" "ValveBiped.Bip01" 0.000005 3.345127 -2.981899 4.944225 89.999982 89.999982 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine1" "ValveBiped.Bip01_Spine" 4.018332 0 0 0 -1.675443 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine2" "ValveBiped.Bip01_Spine1" 3.518565 0 0 0 5.74883 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_Spine4" "ValveBiped.Bip01_Spine2" 8.942641 0.000002 0 0 11.120881 0 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Clavicle" "ValveBiped.Bip01_Spine4" 2.033355 1.000771 -1.93761 73.712037 168.618779 -95.596496 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_UpperArm" "ValveBiped.Bip01_R_Clavicle" 6.028143 -0.000001 0 78.953585 -88.125723 91.732603 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Forearm" "ValveBiped.Bip01_R_UpperArm" 11.692552 0 0 0.000001 -18.575695 0.000004 0 0 0 0 0 0
$definebone "ValveBiped.Bip01_R_Hand" "ValveBiped.Bip01_R_Forearm" 11.4817 -0.000001 0 0.513543 -3.125944 -105.465142 0 0 0 0 0 0
$definebone "ValveBiped.HandControlRotGrandmaR" "" -6.855245 -37.470791 55.472576 -2.905985 -90.838736 -73.89305 0 0 0 0 0 0
$definebone "ValveBiped.HandControlRotParentR" "ValveBiped.HandControlRotGrandmaR" 11.481698 -0.000002 0.000001 6.108074 2.532072 -89.66423 0 0 0 0 0 0
$definebone "ValveBiped.HandControlRotR" "ValveBiped.HandControlRotParentR" 0.000001 0 -0.000002 5.625013 -5.625013 -15.749981 0 0 0 0 0 0
$definebone "ValveBiped.Tip" "ValveBiped.HandControlRotR" 11.1231 -1.935452 -15.150156 -0.537148 -92.19487 -39.209626 0 0 0 0 0 0


$sequence "idle01" {
	"v_crowbar_anims\idle01.smd"
	activity "ACT_VM_IDLE" 1
	fadein 0.2
	fadeout 0.2
	fps 30
	loop
}

$sequence "draw" {
	"v_crowbar_anims\draw.smd"
	activity "ACT_VM_DRAW" 1
	fadein 0.2
	fadeout 0.2
	snap
	fps 30
}

$sequence "misscenter1" {
	"v_crowbar_anims\misscenter1.smd"
	activity "ACT_VM_MISSCENTER" 1
	fadein 0.2
	fadeout 0.2
	snap
	fps 30
}

$sequence "misscenter2" {
	"v_crowbar_anims\misscenter2.smd"
	activity "ACT_VM_MISSCENTER" 1
	fadein 0.2
	fadeout 0.2
	snap
	fps 30
}

$sequence "hitcenter1" {
	"v_crowbar_anims\hitcenter1.smd"
	activity "ACT_VM_HITCENTER" 1
	fadein 0.2
	fadeout 0.2
	snap
	fps 30
}

$sequence "hitcenter2" {
	"v_crowbar_anims\hitcenter2.smd"
	activity "ACT_VM_HITCENTER" 1
	fadein 0.2
	fadeout 0.2
	snap
	fps 30
}

$sequence "hitcenter3" {
	"v_crowbar_anims\hitcenter3.smd"
	activity "ACT_VM_HITCENTER" 1
	fadein 0.2
	fadeout 0.2
	snap
	fps 30
}

$sequence "hitkill1" {
	"v_crowbar_anims\hitkill1.smd"
	activity "ACT_VM_HITKILL" 1
	fadein 0.2
	fadeout 0.2
	snap
	fps 30
}

$sequence "holster" {
	"v_crowbar_anims\holster.smd"
	activity "ACT_VM_HOLSTER" 1
	fadein 0.2
	fadeout 0.2
	fps 30
}
