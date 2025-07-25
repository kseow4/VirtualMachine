

				JMP		Main

#||#####################################################################################||#
#||-------------------------------------------------------------------------------------||#
#||																						||#
#||									int fibonacci(int n)								||#
#||																						||#
#||-------------------------------------------------------------------------------------||#
#||#####################################################################################||#


fibonacci		SUB		R1		R1

				ADI		SP		16				# loading n (R1) used to be 48
				LDR		R1		SP				# FP location
				ADI		SP		-4				# PFP location
				ADI		SP		-4				# n location

				SUB		R2		R2
				LDR		R2		SP				# loading R2 with value of n
				ADI		SP		-4


				SUB		R3		R3				# (n <= 1) ?
				ADI		R3		1

				CMP		R3		R2
				BLT		R3		recursive_fib
				JMP		~fibonacci

												# recursively call fibonacci()

recursive_fib	SUB		R4		R4				# allocating space for local T1-T5 variables
				ADI		R4		-1

				ADI		R2		-1
				STR		R2		SP				# T1	(n - 1)
				ADI		R2		-1
				STR		R2		SP				# T2	(n - 2)

				STR		R4		SP				# T3
				STR		R4		SP				# T4
				STR		R4		SP				# T5	return value

				
#|--------------------------------------------------------------------------------------|#
#|																						|#
#|----------------------------| CREATING ACTIVATION RECORD |----------------------------|#
#|																						|#
#|------------------------------------------|~|-----------------------------------------|#
#|			[ fibonacci(int n - 1) ]		|~|											|#
#|------------------------------------------|~|-----------------------------------------|#

				MOV		R2		SP	
				MOV		R5		R2				# PFP = (R5)

				MOV		R1		FP
				MOV		FP		R2
				ADI		R2		-4
				STR		R1		R2

				ADI		R1		-12				# load (n - 1)
				LDR		R3		R1

				STR		R3		R2
				ADI		R2		-4

				MOV		R7		SL
				CMP		R7		R0
				BGT		R7		OVERFLOW	
				
				MOV		SP		R2
				MOV		R6		PC
				ADI		R6		36
				STR		R6		R5
			
				JMP		fibonacci

#|------------------------------------------|~|-----------------------------------------|#
#|											|~|				[ return (n - 1) ]			|#
#|------------------------------------------|~|-----------------------------------------|#
#|																						|#
#|-----------------------------| END OF ACTIVATION RECORD |-----------------------------|#
#|																						|#
#|--------------------------------------------------------------------------------------|#


				MOV		R5		SP				# placing value returned in R5
				ADI		R5		12				# T3 offset

				SUB		R4		R4
				LDR		R4		SP

				STR		R4		R5


# -------------------------------------------------------------------------------------- #
#							...Now calling fibonacci(n - 2)...							 #
# ______________________________________________________________________________________ #


#|--------------------------------------------------------------------------------------|#
#|																						|#
#|----------------------------| CREATING ACTIVATION RECORD |----------------------------|#
#|																						|#
#|------------------------------------------|~|-----------------------------------------|#
#|			[ fibonacci(int n - 2) ]		|~|											|#
#|------------------------------------------|~|-----------------------------------------|#

				MOV		R2		SP	
				MOV		R5		R2				# PFP = (R5)

				MOV		R1		FP
				MOV		FP		R2
				ADI		R2		-4
				STR		R1		R2

				ADI		R1		-16				# load (n - 2)
				LDR		R3		R1

				STR		R3		R2
				ADI		R2		-4

				MOV		R7		SL
				CMP		R7		R0
				BGT		R7		OVERFLOW	
				
				MOV		SP		R2
				MOV		R6		PC
				ADI		R6		36
				STR		R6		R5
			
				JMP		fibonacci

#|------------------------------------------|~|-----------------------------------------|#
#|											|~|				[ return (n - 2) ]			|#
#|------------------------------------------|~|-----------------------------------------|#
#|																						|#
#|-----------------------------| END OF ACTIVATION RECORD |-----------------------------|#
#|																						|#
#|--------------------------------------------------------------------------------------|#


				MOV		R5		SP				# placing value returned in R5
				ADI		R5		8				# T4 offset

				SUB		R4		R4
				LDR		R4		SP				# R4 contains T4

				STR		R4		R5				# R5 now points to T5 location

				MOV		R3		FP				# R3 contains T3
				ADI		R3		-20
				LDR		R3		R3

				ADD		R4		R3
				STR		R4		R5				# R4 contains T5

				MOV		R0		FP
				LDR		R1		R0

				STR		R4		R0

				MOV		SP		FP
				MOV		R5		SP
				CMP		R5		SB
				BRZ		R5		UNDERFLOW
				BGT		R5		UNDERFLOW

				LDR		FP		R0

				JMR		R1						# return to previous function

#--------------------------------------------------------------------------------------  #
#							...End of recursive function call...						 #
# ______________________________________________________________________________________ #
																						 
												
~fibonacci		MOV		R0		FP				# return to previous function
				LDR		R1		R0
				
				MOV		R2		R0				# loading return value into 
				ADI		R2		-8
				LDR		R2		R2
				STR		R2		R0

				MOV		SP		FP
				MOV		R5		SP
				CMP		R5		SB
				BRZ		R5		UNDERFLOW
				BGT		R5		UNDERFLOW

				LDR		FP		R0
				
				JMR		R1


#||#####################################################################################||#
#||-------------------------------------------------------------------------------------||#
#||#####################################################################################||#		








#||#####################################################################################||#
#||-------------------------------------------------------------------------------------||#
#||																						||#
#||										void Main()										||#
#||																						||#
#||-------------------------------------------------------------------------------------||#
#||#####################################################################################||#


Main			SUB		R0		R0				# Clearing R0 for main use

				RUN		Part1

# -------------------------------------------------------------------------------------- #
#										...Part1...										 #
# ______________________________________________________________________________________ #		

Part1			SUB		R0		R0
				SUB		R5		R5
				
Part1_Input		TRP		4
				MOV		R1		R3

				LDR		R2		enter_key
				CMP		R2		R1
				BRZ		R2		Part1_Fib

				LDR		R2		zero_key
				CMP		R1		R2
				SUB		R0		R0
				MOV		R0		R5
				CMP		R0		R1
				BRZ		R0		~Part1

				SUB		R6		R6
				ADI		R6		10
				MUL		R5		R6				# Shift input by 10

				SUB		R3		R2
				ADD		R5		R3				# Add input

				JMP		Part1_Input


#|--------------------------------------------------------------------------------------|#
#|																						|#
#|----------------------------| CREATING ACTIVATION RECORD |----------------------------|#
#|																						|#
#|------------------------------------------|~|-----------------------------------------|#
#|			[ fibonacci(int n) ]			|~|											|#
#|------------------------------------------|~|-----------------------------------------|#

Part1_Fib		MOV		R0		SP				# creating activation record for fibonacci(n = );
				MOV		R1		R0

				MOV		FP		R0
				ADI		R0		-8
			
				STR		R5		R0
				ADI		R0		-4

				MOV		R7		SL
				CMP		R7		R0
				BGT		R7		OVERFLOW

				MOV		SP		R0
				MOV		R7		PC
				ADI		R7		36
				STR		R7		R1

				JMP		fibonacci

#|------------------------------------------|~|-----------------------------------------|#
#|											|~|				[ return int ]				|#
#|------------------------------------------|~|-----------------------------------------|#
#|																						|#
#|-----------------------------| END OF ACTIVATION RECORD |-----------------------------|#
#|																						|#
#|--------------------------------------------------------------------------------------|#


print_fib		LDB		R3		newline
				TRP		3
				LDB		R3		FF
				TRP		3
				LDB		R3		ii
				TRP		3
				LDB		R3		bb
				TRP		3
				LDB		R3		oo
				TRP		3
				LDB		R3		nn
				TRP		3
				LDB		R3		aa
				TRP		3
				LDB		R3		cc
				TRP		3
				LDB		R3		cc
				TRP		3
				LDB		R3		ii
				TRP		3
				LDB		R3		space
				TRP		3
				LDB		R3		oo
				TRP		3
				LDB		R3		ff
				TRP		3
				LDB		R3		space
				TRP		3						# Fibonacci of_
												# Fibonacci of X is Y
				MOV		R5		SP
				ADI		R5		-8				# loading X into R5
				LDR		R3		R5
				TRP		1						# Fibonacci of X

# -------------------------------------------------------------------------------------- #
#								...Part 2 Adding X...									 #
# ______________________________________________________________________________________ #
																						 #
Part2_Add_Xn	SUB		R1		R1														 #
				SUB		R2		R2														 #
				LDR		R1		p2_array_count											 #
				LDR		R2		p2_array_size											 #
																						 #
				CMP		R2		R1														 #
				BLT		R2		OUT_OF_BOUNDS											 #
				BRZ		R2		OUT_OF_BOUNDS											 #
																						 #
				SUB		R2		R2														 #
				ADI		R2		4														 #
				MUL		R2		R1														 #
																						 #
				LDA		R4		p2_array												 #
				SUB		R4		R2														 #
				STR		R3		R4														 #
																						 #
				ADI		R1		1														 #
				STR		R1		p2_array_count											 #
# ______________________________________________________________________________________ #


				LDB		R3		space
				TRP		3
				LDB		R3		ii
				TRP		3
				LDB		R3		ss
				TRP		3
				LDB		R3		space
				TRP		3						# Fibonacci of X is_

				MOV		R5		SP
				LDR		R3		R5
				TRP		1						# Fibonacci of X is Y


# -------------------------------------------------------------------------------------- #
#								...Part 2 Adding Y...									 #
# ______________________________________________________________________________________ #
																						 #
Part2_Add_Yn	SUB		R1		R1														 #
				SUB		R2		R2														 #
				LDR		R1		p2_array_count											 #
				LDR		R2		p2_array_size											 #
																						 #
				CMP		R2		R1														 #
				BLT		R2		OUT_OF_BOUNDS											 #
				BRZ		R2		OUT_OF_BOUNDS											 #
																						 #
				SUB		R2		R2														 #
				ADI		R2		4														 #
				MUL		R2		R1														 #
																						 #
				LDA		R4		p2_array												 #
				SUB		R4		R2														 #
				STR		R3		R4														 #
																						 #
				ADI		R1		1														 #
				STR		R1		p2_array_count											 #
# ______________________________________________________________________________________ #


				LDB		R3		newline
				TRP		3

				JMP		Part1

~Part1			LDB		R3		newline
				TRP		3


# -------------------------------------------------------------------------------------- #
#						...Part 2 Printing p2_array...									 #
# ______________________________________________________________________________________ #
																						 #
Part2_Print		SUB		R0		R0														 #
				SUB		R1		R1														 #
				SUB		R2		R2														 #
				ADI		R2		-4														 #
				LDR		R1		p2_array_count											 #
				ADI		R1		-1														 #
				MUL		R2		R1														 #
				LDA		R4		p2_array												 #
				ADD		R2		R4														 #
																						 #
Part2_Loop		MOV		R0		R4														 #
				CMP		R0		R2														 #
				BLT		R0		~Part2_Print											 #
				BRZ		R0		~Part2_Print											 #
																						 #
Part2_Print_Xn	LDR		R3		R4														 #
				TRP		1																 #
				LDB		R3		comma													 #
				TRP		3																 #
				LDB		R3		space													 #
				TRP		3																 #
				ADI		R4		-4														 #
																						 #
Part2_Print_Yn	LDR		R3		R2														 #
				TRP		1																 #
				LDB		R3		comma													 #
				TRP		3																 #
				LDB		R3		space													 #
				TRP		3																 #
				ADI		R2		4														 #
																						 #
				JMP		Part2_Loop														 #
																						 #
~Part2_Print	LDB		R3		newline													 #
				TRP		3																 #
# ______________________________________________________________________________________ #


				TRP		0

#||#####################################################################################||#
#||-------------------------------------------------------------------------------------||#
#||#####################################################################################||#		











		

OVERFLOW		LDB		R3		newline
				TRP		3
				TRP		3
				TRP		3
				LDB		R3		oo
				TRP		3
				LDB		R3		vv
				TRP		3
				LDB		R3		ee
				TRP		3
				LDB		R3		rr
				TRP		3	
				LDB		R3		ff
				TRP		3
				LDB		R3		ll
				TRP		3
				LDB		R3		oo
				TRP		3
				LDB		R3		ww
				TRP		3					
				LDB		R3		newline
				TRP		3
				TRP		3
				TRP		0

UNDERFLOW		LDB		R3		newline
				TRP		3
				TRP		3
				TRP		3
				LDB		R3		uu
				TRP		3
				LDB		R3		nn
				TRP		3
				LDB		R3		dd
				TRP		3
				LDB		R3		ee
				TRP		3
				LDB		R3		rr
				TRP		3			
				LDB		R3		ff
				TRP		3
				LDB		R3		ll
				TRP		3
				LDB		R3		oo
				TRP		3
				LDB		R3		ww
				TRP		3					
				LDB		R3		newline
				TRP		3
				TRP		3
				TRP		0

OUT_OF_BOUNDS	LDB		R3		newline
				TRP		3
				TRP		3
				TRP		3
				LDB		R3		OO
				TRP		3
				LDB		R3		uu
				TRP		3
				LDB		R3		tt
				TRP		3
				LDB		R3		space
				TRP		3
				LDB		R3		OO
				TRP		3			
				LDB		R3		ff
				TRP		3
				LDB		R3		space
				TRP		3
				LDB		R3		BB
				TRP		3
				LDB		R3		oo
				TRP		3	
				LDB		R3		uu
				TRP		3	
				LDB		R3		nn
				TRP		3	
				LDB		R3		dd
				TRP		3	
				LDB		R3		ss
				TRP		3	
				LDB		R3		newline
				TRP		3
				TRP		3
				TRP		0


# ----------------------------- #
#		...DIRECTIVES...		#
# _____________________________ #

aa				.BYT	'a'
bb				.BYT	'b'
cc				.BYT	'c'
dd				.BYT	'd'
ee				.BYT	'e'
ff				.BYT	'f'
gg				.BYT	'g'
hh				.BYT	'h'
ii				.BYT	'i'
jj				.BYT	'j'
kk				.BYT	'k'
ll				.BYT	'l'
mm				.BYT	'm'
nn				.BYT	'n'
oo				.BYT	'o'
pp				.BYT	'p'
qq				.BYT	'q'
rr				.BYT	'r'
ss				.BYT	's'
tt				.BYT	't'
uu				.BYT	'u'
vv				.BYT	'v'
ww				.BYT	'w'
xx				.BYT	'x'
yy				.BYT	'y'
zz				.BYT	'z'

AA				.BYT	'A'
BB				.BYT	'B'
CC				.BYT	'C'
DD				.BYT	'D'
EE				.BYT	'E'
FF				.BYT	'F'
GG				.BYT	'G'
HH				.BYT	'H'
II				.BYT	'I'
JJ				.BYT	'J'
KK				.BYT	'K'
LL				.BYT	'L'
MM				.BYT	'M'
NN				.BYT	'N'
OO				.BYT	'O'
PP				.BYT	'P'
QQ				.BYT	'Q'
RR				.BYT	'R'
SS				.BYT	'S'
TT				.BYT	'T'
UU				.BYT	'U'
VV				.BYT	'V'
WW				.BYT	'W'
XX				.BYT	'X'
YY				.BYT	'Y'
ZZ				.BYT	'Z'

stop_symbol		.BYT	'@'
positive		.BYT	'+'
negative		.BYT	'-'
question		.BYT	'?'
exclaim			.BYT	'!'
period			.BYT	'.'
comma			.BYT	','
divide			.BYT	'/'
newline			.BYT	'\n'
space			.BYT	' '

enter_key		.INT	13
zero_key		.INT	48
one_key			.INT	49
two_key			.INT	50
three_key		.INT	51
four_key		.INT	52
five_key		.INT	53
six_key			.INT	54
seven_key		.INT	55
eight_key		.INT	56
nine_key		.INT	57

# ----------------------------- #
#		Global Variables:		#
# _____________________________ #

p2_array_count	.INT	0
p2_array_size	.INT	30

p2_array		.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0		# ARRAY[9]
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0		# ARRAY[19]
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0
				.INT	0		# ARRAY[29]

#| locks

Lock_A			.INT	-1
Lock_B			.INT	-1
Lock_C			.INT	-1
Lock_D			.INT	-1
Lock_E			.INT	-1
Lock_F			.INT	-1
Lock_G			.INT	-1
Lock_H			.INT	-1
Lock_I			.INT	-1
Lock_J			.INT	-1
Lock_K			.INT	-1