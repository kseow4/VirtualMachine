#PART 1
				ADI		R7		4
LOOP1_START		LDR		R1		i				#12
				LDR		R4		SIZE			#24
				CMP		R4		R1				#36
				BRZ		R4		LOOP1_END		#48
#Load sum and array
				LDR		R2		sum				#60
				LDA		R5		ARR				#72
#calculate array index and add to sum
				LDR		R0		i
				MUL		R0		R7
				SUB		R5		R0				#84
				LDR		R5		R5				#96
				ADD		R2		R5				#108
#store sum
				STR		R2		sum				#120
#mod 2 current index
				MOV		R2		R5				#132
				LDR		R6		modtwo			#144
				DIV		R2		R6				#156
				MUL		R2		R6				#168
				CMP		R2		R5				#180
				BRZ		R2		EVEN_NUMBER		#192
				JMP		ODD_NUMBER				#204

LOOP_INCREMENT	ADI		R1		1				#216
				STR		R1		i				#228
				JMP		LOOP1_START				#240

EVEN_NUMBER		MOV		R3		R5				#252
				TRP		1						#264
				LDB		R3		space			#276
				TRP		3						#288
				LDB		R3		ii				#300
				TRP		3						#312
				LDB		R3		ss				#324
				TRP		3						#336
				LDB		R3		space			#348
				TRP		3						#360
				LDB		R3		ee				#372
				TRP		3						#384
				LDB		R3		vv				#396
				TRP		3						#408
				LDB		R3		ee				#420
				TRP		3						#432
				LDB		R3		nn				#444
				TRP		3						#456
				LDB		R3		newLine			#468
				TRP		3						#480
				JMP		LOOP_INCREMENT			#492

ODD_NUMBER		MOV		R3		R5				#504
				TRP		1						#516
				LDB		R3		space			#528
				TRP		3						#540
				LDB		R3		ii				#552
				TRP		3						#564
				LDB		R3		ss				#576
				TRP		3						#588
				LDB		R3		space			#600
				TRP		3						#612
				LDB		R3		oo				#624
				TRP		3						#636
				LDB		R3		dd				#648
				TRP		3						#660
				TRP		3						#672
				LDB		R3		newLine			#684
				TRP		3						#696
				JMP		LOOP_INCREMENT			#708

LOOP1_END		LDB		R3		SS				#720
				TRP		3						#732
				LDB		R3		uu				#744
				TRP		3						#756
				LDB		R3		mm				#768
				TRP		3						#780
				LDB		R3		space			#792
				TRP		3						#804
				LDB		R3		ii				#816
				TRP		3						#828
				LDB		R3		ss				#840
				TRP		3						#852
				LDB		R3		space			#864
				TRP		3						#876
				LDR		R3		sum				#888
				TRP		1						#900
				LDB		R3		newLine			#912
				TRP		3						#924
				TRP		3
				JMP		PART_2_START
#only part 1	TRP		0						#948

#PART 2
PART_2_START	LDR		R0		zero
				LDR		R1		zero
				LDR		R2		zero
				LDR		R3		zero
				LDR		R4		zero
				LDR		R5		zero
				LDR		R6		zero
#zeroed out registers

				LDA		R1		DAGS
				LDA		R2		GADS
#load GADS with DAGS
				LDR		R2		R1
				STR		R2		GADS
#swap D and G
				LDA		R4		DAGS
				ADI		R4		-2
				LDA		R6		GADS
				STB		R4		R6
				LDA		R4		DAGS
				ADI		R6		-2
				STB		R4		R6

LOOP2_START		LDR		R0		j
				LDR		R7		LENGTH
				CMP		R7		R0
				BRZ		R7		LOOP2_END
				
				LDA		R1		GADS
				LDA		R2		DAGS
				SUB		R1		R0
				SUB		R2		R0

				LDB		R5		R1
				LDB		R6		R2
				CMP		R5		R6
				MOV		R7		R5
				JMP		PRINT_LOOP2

DAGS_<_GADS		LDB		R3		relLT
				TRP		3
				JMP		FINISH_PRINT2

DAGS_>_GADS		LDB		R3		relGT
				TRP		3
				JMP		FINISH_PRINT2

DAGS_=_GADS		LDB		R3		relEQ
				TRP		3
				JMP		FINISH_PRINT2

LOOP2_INCREMENT	ADI		R0		1
				STR		R0		j
				JMP		LOOP2_START

PRINT_LOOP2		LDB		R3		R2
				TRP		3
				LDB		R3		space
				TRP		3
				BLT		R7		DAGS_>_GADS
				BGT		R7		DAGS_<_GADS
				BRZ		R7		DAGS_=_GADS
FINISH_PRINT2	LDB		R3		space
				TRP		3
				LDB		R3		R1
				TRP		3
				LDB		R3		Dash
				TRP		3
				TRP		3
				JMP		LOOP2_INCREMENT

LOOP2_END		LDB		R3		newLine
				TRP		3
				TRP		3
				LDR		R3		DAGS
				TRP		1
				LDB		R3		space
				TRP		3
				LDB		R3		Dash
				TRP		3
				LDB		R3		space
				TRP		3
				LDR		R3		GADS
				TRP		1
				LDB		R3		space
				TRP		3
				LDB		R3		relEQ
				TRP		3
				LDB		R3		space
				TRP		3
				LDR		R1		GADS
				LDR		R2		DAGS
				SUB		R2		R1
				MOV		R3		R2
				TRP		1
				LDB		R3		newLine
				TRP		3
				TRP		0

#Part 1 Arrays
zero	.INT	0
modtwo	.INT	2
i		.INT	0
sum		.INT	0
temp	.INT	0
result	.INT	0
SIZE	.INT	10
ARR		.INT	10
		.INT	2
		.INT	3
		.INT	4
		.INT	15
		.INT	-6
		.INT	7
		.INT	8
		.INT	9
		.INT	10
ii		.BYT	'i'
ss		.BYT	's'
nn		.BYT	'n'
vv		.BYT	'v'
ee		.BYT	'e'
oo		.BYT	'o'
dd		.BYT	'd'
SS		.BYT	'S'
uu		.BYT	'u'
mm		.BYT	'm'
comma	.BYT	','
space	.BYT	' '
newLine	.BYT	'\r'

#Part 2 DAGS
DAGS	.BYT	'D'
		.BYT	'A'
		.BYT	'G'
		.BYT	'S'
GADS	.INT	-99
relLT	.BYT	'<'
relGT	.BYT	'>'
relEQ	.BYT	'='
Dash	.BYT	'-'
j		.INT	0
LENGTH	.INT	4




