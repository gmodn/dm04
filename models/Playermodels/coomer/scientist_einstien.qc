
$ModelName "player/HL1/edit/scientist_einstien.mdl"

$Model "scientist_einstien" "scientist_einstien_ref.smd" {

	$Include "scientist_einstien_flex.qci"
}

$Include "scientist_einstien_lod.qci"

$SurfaceProp "flesh"

$Contents "solid"

$IllumPosition -0.444 -0.007 35.329

$EyePosition 0 0 70

$CDMaterials "models\scientist\"
$CDMaterials ""

$Attachment "eyes" "ValveBiped.Bip01_Head1" 3.43 -3.51 0.05 rotate 0 -80.1 -90
$Attachment "mouth" "ValveBiped.Bip01_Head1" 0.8 -5.5 0.1 rotate 0 -80 -90
$Attachment "chest" "ValveBiped.Bip01_Spine2" 5 4 0 rotate 0 90 90
$Attachment "forward" "ValveBiped.forward" 0 0 0 rotate 0 0 0
$Attachment "anim_attachment_RH" "ValveBiped.Anim_Attachment_RH" 0 0 0 rotate -90 -90 0
$Attachment "anim_attachment_LH" "ValveBiped.Anim_Attachment_LH" 0 0 0 rotate -90 -90 0
$Attachment "anim_attachment_head" "ValveBiped.Bip01_Head1" 0 0 0 rotate -90 -90 0

// $Include "scientist_einstien_box.qci"

$Include "scientist_einstien_bone.qci"

$Include "scientist_einstien_animation.qci"

$Include "scientist_einstien_collision.qci"
