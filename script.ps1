$mnems = ""

foreach($name in $args)
{
	$mnems += get-mailbox $name | select samaccountname
}

returns $mnems
