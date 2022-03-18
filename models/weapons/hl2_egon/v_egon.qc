
$ModelName "weapons/v_egon.mdl"

$BodyGroup "body"
{
	studio "v_egon_reference.smd"
}

$Include "v_egon_lod.qci"

$SurfaceProp "default"

$Contents "solid"

$IllumPosition -1.671 -10.612 -14.078

$CDMaterials "models\Weapons\V_hand\"
$CDMaterials "models\weapons\v_egon\"

$Attachment "muzzle" "egon" 0 0 17 rotate -90 0 0

// $Include "v_egon_box.qci"

$Include "v_egon_bone.qci"

$Include "v_egon_animation.qci"
