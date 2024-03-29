extends Node2D


func _on_grass_body_entered(_body):
	print('oh no, i am being eaten')
	$grass01.amount = $grass01.amount - 1
	print('i have ', $grass01.amount, ' grass left')


func _on_grass_02_body_entered(_body):
	$grass02.amount = $grass02.amount - 1
	print('i have ', $grass02.amount, ' grass left')


func _on_grass_03_body_entered(_body):
	$grass03.amount = $grass03.amount - 1
	print('i have ', $grass03.amount, ' grass left')
