// Created by Crowbar 0.71

$modelname "Weapons/rifleshell.mdl"

$bodygroup "studio"
{
	studio "rifleshell.smd"
}


$surfaceprop "default"

$contents "solid"

$illumposition 0 0.002 0.543

$cdmaterials "Models\Weapons\rifle_shell\"

$cbox 0 0 0 0 0 0

$bbox -2.265 -0.584 -0.05 2.262 0.584 1.136

$definebone "Cylinder02" "" 0 0.001609 0.543281 0.000002 179.999855 -0.000005 0 0 0 0 0 0


$sequence "shell" {
	"rifleshell_anims\shell.smd"
	fadein 0.2
	fadeout 0.2
	fps 30
}
