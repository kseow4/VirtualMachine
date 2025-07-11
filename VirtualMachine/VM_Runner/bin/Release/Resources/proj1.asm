BEGIN	LDB		R3		KK
		TRP		R3
		LDB		R3		uu
		TRP		R3
		LDB		R3		pp
		TRP		R3
		LDB		R3		cc
		TRP		R3
		LDB		R3		hh
		TRP		R3
		LDB		R3		oo
		TRP		R3
		LDB		R3		comma
		TRP		R3
		LDB		R3		space
		TRP		R3
		LDB		R3		KK
		TRP		R3
		LDB		R3		yy
		TRP		R3
		LDB		R3		ll
		TRP		R3
		LDB		R3		ee
		TRP		R3
		LDB		R3		newLine
		TRP		R3
		TRP		R3

ADD_B	LDR		R2		B1
		MOV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R4		B2
		ADD		R2		R4
		MOV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R4		B3
		ADD		R2		R4
		MOV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R4		B4
		ADD		R2		R4
		MOV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R4		B5
		ADD		R2		R4
		MOV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R4		B6
		ADD		R2		R4
		MOV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		MOV		R7		R2
		LDB		R3		newLine
		TRP		3
		TRP		3
MUL_A	LDR		R2		A1
		MOV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R4		A2
		MUL		R2		R4
		MOV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R4		A3
		MUL		R2		R4
		MOV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R4		A4
		MUL		R2		R4
		MOV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R4		A5
		MUL		R2		R4
		MOV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R4		A6
		MUL		R2		R4
		MOV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		MOV		R6		R2
		LDB		R3		newLine
		TRP		3
		TRP		3
DIV_B	LDR		R2		B1
		MOV		R3		R7
		DIV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R2		B2
		MOV		R3		R7
		DIV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R2		B3
		MOV		R3		R7
		DIV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R2		B4
		MOV		R3		R7
		DIV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R2		B5
		MOV		R3		R7
		DIV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R2		B6
		MOV		R3		R7
		DIV		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		MOV		R7		R2
		LDB		R3		newLine
		TRP		3
		TRP		3
SUB_C	LDR		R2		C1
		MOV		R3		R6
		SUB		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R2		C2
		MOV		R3		R6
		SUB		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R2		C3
		MOV		R3		R6
		SUB		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDR		R2		C4
		MOV		R3		R6
		SUB		R3		R2
		TRP		1
		LDB		R3		space
		TRP		3
		TRP		3
		LDB		R3		newLine
		TRP		3
		TRP		3
		TRP		0

A1		.INT	1
A2		.INT	2
A3		.INT	3
A4		.INT	4
A5		.INT	5
A6		.INT	6
B1		.INT	300
B2		.INT	150
B3		.INT	50
B4		.INT	20
B5		.INT	10
B6		.INT	5
C1		.INT	500
C2		.INT	2
C3		.INT	5
C4		.INT	10
KK		.BYT	'K'
uu		.BYT	'u'
pp		.BYT	'p'
cc		.BYT	'c'
hh		.BYT	'h'
oo		.BYT	'o'
yy		.BYT	'y'
ll		.BYT	'l'
ee		.BYT	'e'
comma	.BYT	','
space	.BYT	' '
newLine	.BYT	'\r'

