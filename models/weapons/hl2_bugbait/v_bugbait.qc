
$ModelName "weapons/V_bugbait.mdl"

$BodyGroup "studio"
{
	studio "v_bugbait_Bugbait_reference.smd"
}

$Include "v_bugbait_lod.qci"

$SurfaceProp "default"

$Contents "solid"

$IllumPosition -8.144 -14.448 -16.05

$CDMaterials "models\Weapons\V_hand\"
$CDMaterials "models\Weapons\V_bugbait\"

$Attachment "0" "ValveBiped.Bip01_R_Hand" 4 -4 0 rotate 0 0 0

// $Include "v_bugbait_box.qci"

$Include "v_bugbait_bone.qci"

$Include "v_bugbait_animation.qci"
