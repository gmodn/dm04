
$ModelName "weapons/w_gauss.mdl"

$BodyGroup "Body"
{
	studio "tau_reference.smd"
}

$Include "w_gauss_lod.qci"

$SurfaceProp "metal"

$Contents "solid"

$IllumPosition -0.041 -3.216 4.19

$CDMaterials "models\Buggy\"

// $Include "w_gauss_box.qci"

$Include "w_gauss_bone.qci"

$Include "w_gauss_animation.qci"

$Include "w_gauss_collision.qci"

$KeyValues
{
	prop_data 
	{
		"base"		"Metal.Small"
	}
}
