
$ModelName "weapons/v_gauss.mdl"

$BodyGroup "studio"
{
	studio "v_gauss_gauss_gun.smd"
}
$BodyGroup "hands"
{
	studio "RefHEVHands.smd"
}

$Include "v_gauss_lod.qci"

$Contents "solid"

$IllumPosition 54.64 -10.537 -17.875

$CDMaterials "models\Weapons\V_hand\"
$CDMaterials "models\Weapons\V_Gauss\"

$Attachment "muzzle" "g_weapon" 25 0 0 rotate 0 0 90
$Attachment "core" "g_weapon" 6 0 0 rotate 0 0 90
$Attachment "core_spin" "g_spin" 3 0 0 rotate 0 -180 0
$Attachment "fan" "g_fan" 0 0 0 rotate 0 -180 0

// $Include "v_gauss_box.qci"

$Include "v_gauss_bone.qci"

$Include "v_gauss_animation.qci"
