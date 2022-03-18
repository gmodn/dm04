
$ModelName "weapons/v_superphyscannon.mdl"

$BodyGroup "studio"
{
	studio "v_superphyscannon_SuperPhyscannon_reference_fixed.smd"
}

$Include "v_superphyscannon_lod.qci"

$SurfaceProp "default"

$Contents "solid"

$IllumPosition -8.528 -43.69 -11.292

$CDMaterials "models\weapons\v_physcannon\"
$CDMaterials "models\weapons\v_hand\"

$Attachment "muzzle" "Muzzle" 0 0.25 -2 rotate 0 0 0
$Attachment "fork1b" "Base_A" 0 0 0 rotate 0 0 0
$Attachment "fork1m" "Prong_A" 0 0 0 rotate 0 0 0
$Attachment "fork1t" "Tip_A" 0 0 0 rotate 0 0 0
$Attachment "fork2b" "Base_B" 0 0 0 rotate 0 0 0
$Attachment "fork2m" "Prong_B" 0 0 0 rotate 0 0 0
$Attachment "fork2t" "Tip_B" 0 0 0 rotate 0 0 0

// $Include "v_superphyscannon_box.qci"

$Include "v_superphyscannon_bone.qci"

$Include "v_superphyscannon_animation.qci"
