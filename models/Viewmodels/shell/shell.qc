// Created by Crowbar 0.71

$modelname "Weapons/shell.mdl"

$bodygroup "studio"
{
	studio "shell.smd"
}


$surfaceprop "default"

$contents "solid"

$illumposition -0.008 -0.001 0.548

$cdmaterials "Models\Weapons\pistol_shell\"

$cbox 0 0 0 0 0 0

$bbox -0.935 -0.548 0 0.936 0.532 1.095

$definebone "shell" "" 0 0 0 0 0 89.999982 0 0 0 0 0 0


$sequence "shell" {
	"shell_anims\shell.smd"
	fadein 0.2
	fadeout 0.2
	fps 30
}
