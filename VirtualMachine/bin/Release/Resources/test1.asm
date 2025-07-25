# Check even or odd:

		TRP		2
		MOV		R1		R3
		TRP		1		
		LDB		R3		newLine
		TRP		3
		TRP		4
		MOV		R2		R3
		TRP		3




#		ADI		R4		-5
#		ADI		R1		-5
#		ADI		R2		3
#		LDR		R6		modtwo		
#		DIV		R1		R2			
#		MUL		R1		R2				
#		MOV		R3		R1
#		TRP		1



		LDB		R3		newLine
		TRP		3
		TRP		0


i		.INT	0
sum		.INT	0
temp	.INT	0
result	.INT	0
SIZE	.INT	10
ARR		.INT	0
		.INT	1
		.INT	2
		.INT	3
		.INT	4
		.INT	5
		.INT	6
		.INT	7
		.INT	8
		.INT	9

modtwo	.INT	2
iBB		.INT	0
iBA		.INT	0
iAC		.INT	102
iAD		.INT	7
iA		.INT	3
iB		.INT	2
ii		.BYT	'i'
ss		.BYT	's'
nn		.BYT	'n'
vv		.BYT	'v'
ee		.BYT	'e'
comma	.BYT	','
space	.BYT	' '
newLine	.BYT	'\r'