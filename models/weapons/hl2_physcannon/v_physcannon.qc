
$ModelName "weapons/v_physcannon.mdl"

$BodyGroup "studio"
{
	studio "v_physcannon_reference.smd"
}

$Include "v_physcannon_lod.qci"

$SurfaceProp "default"

$Contents "solid"

$IllumPosition -8.508 -43.551 -11.368

$CDMaterials "models\weapons\v_physcannon\"
$CDMaterials "models\weapons\v_hand\"

$Attachment "muzzle" "Muzzle" 0 0.25 -2 rotate 0 0 0
$Attachment "fork1b" "Base_A" 0 0 0 rotate 0 0 0
$Attachment "fork1m" "Prong_A" 0 0 0 rotate 0 0 0
$Attachment "fork1t" "Tip_A" 0 0 0 rotate 0 0 0
$Attachment "fork2b" "Base_B" 0 0 0 rotate 0 0 0
$Attachment "fork2m" "Prong_B" 0 0 0 rotate 0 0 0
$Attachment "fork2t" "Tip_B" 0 0 0 rotate 0 0 0

// $Include "v_physcannon_box.qci"

$Include "v_physcannon_bone.qci"

$Include "v_physcannon_animation.qci"
