

#Find Main() and start there.
			JMP		Main



opd			MOV		R0		SP				# getting local variables
			SUB		R1		R1
			STR		R1		R0
			ADI		SP		-4

			ADI		R0		12				# 
			LDR		R1		R0

			ADI		R0		4				# c[0]
			LDR		R2		R0
			ADI		R0		4				# tenth
			LDR		R4		R0
			ADI		R0		4				# c[cnt]
		








#||#####################################################################################||#
#||-------------------------------------------------------------------------------------||#
#||																						||#
#||										void flush()									||#
#||																						||#
#||-------------------------------------------------------------------------------------||#
#||#####################################################################################||#

flush		SUB		R1		R1
			STR		R1		data
			TRP		4
			STB		R3		c
			TRP		99
flushloop	SUB		R2		R2
			STB		R2		newline
			CMP		R2		R3
			BRZ		R2		~flush
			TRP		4
			STB		R3		c
			JMP		flushloop

~flush		MOV		SP		FP				# De-allocate Current Activation Record
			MOV		R7		SP
			CMP		R7		SB
			BGT		R7		UNDERFLOW

			LDR		R7		FP				# Return Address Pointed to by FP		
			MOV		R0		FP
			ADI		R0		-4
			LDR		FP		FP
			JMR		R7

			TRP		0						# Ends the program should it reach this state


#||#####################################################################################||#
#||-------------------------------------------------------------------------------------||#
#||																						||#
#||										void getdata()									||#
#||																						||#
#||-------------------------------------------------------------------------------------||#
#||#####################################################################################||#


getdata		LDR		R0		count					
			LDR		R2		SIZE
			CMP		R2		R0
			BLT		R2		toobig			

			LDA		R4		array
			ADD		R4		R0
			TRP		2
			STR		R4		R3
			ADI		R0		1
			STR		R0		cnt
			JMP		~getdata

toobig		LDB		R3		newline
			TRP		3
			LDB		R3		NN
			TRP		3
			LDB		R3		uu
			TRP		3
			LDB		R3		mm
			TRP		3
			LDB		R3		bb
			TRP		3
			LDB		R3		ee
			TRP		3			
			LDB		R3		rr
			TRP		3
			LDB		R3		space
			TRP		3
			LDB		R3		tt
			TRP		3
			LDB		R3		oo
			TRP		3		
			TRP		3
			LDB		R3		space		
			TRP		3
			LDB		R3		BB
			TRP		3
			LDB		R3		ii
			TRP		3
			LDB		R3		gg
			TRP		3
			LDB		R3		newline
			TRP		3


#|--------------------------------------------------------------------------------------|#
#|																						|#
#|----------------------------| CREATING ACTIVATION RECORD |----------------------------|#
#|																						|#
#|------------------------------------------|~|-----------------------------------------|#
#|				[ flush() ]					|~|											|#
#|------------------------------------------|~|-----------------------------------------|#

		TRP		99
			MOV		R0		SP				# checking for overflow
			ADI		R0		-8
			CMP		R0		SL
			BLT		R0		OVERFLOW

			MOV		R0		SP				# flush();
			MOV		FP		SP

			ADI		SP		-4				# Adjust Stack Pointer to new Top of Stack
			STR		R0		SP				# PFP to Top of Stack
			
			ADI		SP		-4				# Adjust Stack Pointer to New Top
								
			MOV		R1		PC				# Compute Return Address
			ADI		R1		36				# size was 36 bytes
			STR		R1		FP

			JMP		flush					# Return value will be the next instruction

#|------------------------------------------|~|-----------------------------------------|#
#|											|~|				[ return void ]				|#
#|------------------------------------------|~|-----------------------------------------|#
#|																						|#
#|-----------------------------| END OF ACTIVATION RECORD |-----------------------------|#
#|																						|#
#|--------------------------------------------------------------------------------------|#


~getdata	MOV		SP		FP				# De-allocate Current Activation Record
			MOV		R7		SP
			CMP		R7		SB
			BGT		R7		UNDERFLOW

			LDR		R7		FP				# Return Address Pointed to by FP
			LDR		R0		FP
			ADI		R0		-4
			LDR		FP		R0

		#	LDR		R0		FP
		#	ADI		R0		-4
		#	LDR		FP		R0
		TRP		99
			JMR		R7

			TRP		0						# Ends the program should it reach this state


#||#####################################################################################||#
#|| ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ||#
#||#####################################################################################||#









#||#####################################################################################||#
#||-------------------------------------------------------------------------------------||#
#||																						||#
#||							void reset(int w, int x, int y, int z)						||#
#||																						||#
#||-------------------------------------------------------------------------------------||#
#||#####################################################################################||#


reset		MOV		R0		FP		
			ADI		R0		-8				# param int w (R3)
			LDR		R3		R0	
			STR		R3		data
	
			ADI		R0		-4				# param int x (R4)
			LDR		R4		R0
			STR		R4		opdv
		
			ADI		R0		-4				# param int y (R5)
			LDR		R5		R0
			STR		R5		cnt

			ADI		R0		-4				# param int z (R6)
			LDR		R6		R0
			STR		R6		flag

			ADI		R0		-4				# local variable int k (R7)
			LDR		R7		R0

resetloop	LDA		R2		c				# for (k = 0; k < SIZE; k++)
			LDR		R1		SIZE
			CMP		R1		R7				# int k (R7)
			BLT		R1		~reset

			SUB		R2		R7

			SUB		R0		R0
			STB		R2		R0
			ADI		R7		1
			JMP		resetloop

~reset		MOV		SP		FP				# De-allocate Current Activation Record
			MOV		R7		SP
			CMP		R7		SB
			BGT		R7		UNDERFLOW

			LDR		R7		FP				# Return Address Pointed to by FP
			LDR		R0		FP
			ADI		R0		-4
			LDR		FP		R0


		#	LDR		R0		FP
		#	ADI		R0		-4
		#	LDR		FP		R0
			TRP		99
			JMR		R7

			TRP		0						# Ends the program should it reach this state



#||#####################################################################################||#
#|| ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ||#
#||#####################################################################################||#










#||#####################################################################################||#
#||-------------------------------------------------------------------------------------||#
#||																						||#
#||										void Main()										||#
#||																						||#
#||-------------------------------------------------------------------------------------||#
#||#####################################################################################||#


Main		SUB		R0		R0				# Clearing R0 for main use


#|--------------------------------------------------------------------------------------|#
#|																						|#
#|----------------------------| CREATING ACTIVATION RECORDS |---------------------------|#
#|																						|#
#|--------------------------------------------------------------------------------------|#

#|------------------------------------------|~|-----------------------------------------|#
#|	[ reset(int w, int x, int y, int z) ]	|~|											|#
#|------------------------------------------|~|-----------------------------------------|#

			MOV		R0		SP				# checking for overflow
			ADI		R0		-8
			CMP		R0		SL
			BLT		R0		OVERFLOW
			
			MOV		R0		FP				# Creating activation record for reset(w = 1, x = 0, y = 0, z = 0);
			MOV		FP		SP

			ADI		SP		-4				# Adjust Stack Pointer to new Top of Stack
			STR		R0		SP				# PFP to Top of Stack

			SUB		R1		R1				# Pass parameters onto the stack (pass by value)					
			ADI		R1		1				# passing 1
			ADI		SP		-4

			STR		R1		SP				# loading w (0)	
			ADI		SP		-4

			SUB		R1		R1				# passing 0's

			STR		R1		SP				# loading x (0)
			ADI		SP		-4	
			
			STR		R1		SP				# loading y (0)
			ADI		SP		-4

			STR		R1		SP				# loading z (0)
			ADI		SP		-4				# Done loading parameters
											# Adjust Stack Pointer to New Top
			MOV		R1		PC				# Compute Return Address
			ADI		R1		36				# size was 48 bytes (4 instruction sets) -> no
			STR		R1		FP

			JMP		reset					# Return value will be the next instruction

#|------------------------------------------|~|-----------------------------------------|#
#|											|~|				[ return void ]				|#
#|------------------------------------------|~|-----------------------------------------|#

			


#|------------------------------------------|~|-----------------------------------------|#
#|				[ getdata() ]				|~|											|#
#|------------------------------------------|~|-----------------------------------------|#

			MOV		R0		SP				# checking for overflow
			ADI		R0		-8
			CMP		R0		SL
			BLT		R0		OVERFLOW

			MOV		R0		FP				# Creating activation record for getdata();
			MOV		FP		SP
			ADI		SP		-4				# Adjust Stack Pointer to new Top of Stack

			STR		R0		SP				# PFP to Top of Stack
			ADI		SP		-4				# Adjust Stack Pointer to New Top

			MOV		R1		PC				# Compute Return Address
			ADI		R1		36				# size was 36 bytes
			STR		R1		FP

			TRP		99

			JMP		getdata

#|------------------------------------------|~|-----------------------------------------|#
#|											|~|				[ return void ]				|#
#|------------------------------------------|~|-----------------------------------------|#
			
			TRP		99
			ADI		SB		0
			TRP		99

			






while1		LDA		R6		c				# while c[0] != '@'
			LDR		R7		stop_symbol
			SUB		R5		R5
			CMP		R7		R6
			CMP		R7		R5
			BRZ		R7		end_of_main
			LDR		R5		positive		# if c[0] == '+' || c[0] =='-'
			LDR		R7		negative
			CMP		R7		R6
			BRZ		R7		getnext
			CMP		R5		R6
			BNZ		R5		getsign
											# if true:
getnext		MOV		R0		SP				# create activation record for getdata();
			MOV		R1		R0
			ADI		R0		-4
			MOV		FP		R0
			ADI		R0		-4
			MOV		R4		SL
			CMP		R4		R0
			BGT		R4		OVERFLOW
			MOV		SP		R0
			MOV		R4		PC
			ADI		R4		24
			STR		R4		R1
			JMP		getdata					# calling getdata();
			MOV		SP		FP				# De-Allocation
			MOV		R5		SP
			CMP		R5		SB
			BGT		R5		UNDERFLOW
			LDR		R5		FP
			ADI		R5		-4
			LDR		FP		R5

											#if false:
getsign		LDA		R6		c
			LDA		R7		c
			ADI		R7		1
			STB		R7		R6
			STB		R6		positive
			LDR		R5		cnt
			ADI		R5		1
			STR		R5		cnt

while2		LDR		R7		data
			BRZ		R7		end_of_main
			LDA		R6		c						# if c[cnt - 1] == '\n'
			LDR		R5		cnt	
			ADI		R5		-1
			ADD		R6		R5
			LDR		R4		newline
			CMP		R6		R4
			BNZ		R6		while2else
			SUB		R7		R7
			STR		R7		data
			ADI		R7		1
			STR		R7		tenth
			LDR		R7		cnt
			ADI		R7		-2
			STR		R7		cnt

while3		LDR		R7		flag
			BNZ		R7		while3break
			LDR		R6		cnt
			BRZ		R6		while3break
													# call opd(c[0], tenth, c[cnt]);
			TRP		99

			MOV		R0		SP						# making activation record for opd();
			MOV		R1		R0
			ADI		R0		-4
			MOV		FP		R0

			ADI		R0		-4
			SUB		R2		R2
			STR		R2		R0
		
			SUB		R2		R2
			LDB		R2		c
			STR		R2		R0
			ADI		R0		-4
			STR		R2		R0
			ADI		R0		-4
			STR		R2		R0
			ADI		R0		-4
			STR		R2		R0

			MOV		R4		SL
			CMP		R4		R0
			BGT		R4		OVERFLOW

			MOV		SP		R0
			MOV		R4		PC
			ADI		R4		20				# might be 48?
			STR		R4		R1
			JMP		opd						# calling opd
			MOV		SP		FP
			MOV		R5		SP
			CMP		R5		SB
			BGT		R5		UNDERFLOW
			LDR		R5		FP
			ADI		R5		-4
			LDR		FP		R5


												# end of while3

while3break	LDR		R7		flag
			BRZ		R7		while2else			
			LDB		R3		OO					# printf "Operand is %d\n", opdv
			TRP		3
			LDB		R3		pp
			TRP		3
			LDB		R3		ee
			TRP		3
			LDB		R3		rr
			TRP		3	
			LDB		R3		aa
			TRP		3
			LDB		R3		nn
			TRP		3
			LDB		R3		dd
			TRP		3
			LDB		R3		space
			TRP		3					
			LDB		R3		ii
			TRP		3
			LDB		R3		ss
			TRP		3
			LDB		R3		space
			TRP		3
			LDR		R3		opdv
			TRP		1
			LDB		R3		newline
			TRP		3					

while2else	MOV		R0		SP				# create activation record for getdata();
			MOV		R1		R0
			ADI		R0		-4
			MOV		FP		R0
			ADI		R0		-4
			MOV		R4		SL
			CMP		R4		R0
			BGT		R4		OVERFLOW
			MOV		SP		R0
			MOV		R4		PC
			ADI		R4		24
			STR		R4		R1
			JMP		getdata					# calling getdata();
			MOV		SP		FP				# De-Allocation
			MOV		R5		SP
			CMP		R5		SB
			BGT		R5		UNDERFLOW
			LDR		R5		FP
			ADI		R5		-4
			LDR		FP		R5
											# end of while2

			MOV		R0		SP				# creating activation records...
			MOV		R1		R0				# reset(1, 0, 0, 0);
			ADI		R0		-4
			MOV		FP		R0
			ADI		R0		-4
			SUB		R2		R2
			STR		R2		R0
			ADI		R0		-4
			SUB		R2		R2
			STR		R2		R0
			ADI		R0		-4
			STR		R2		R0
			ADI		R0		-4
			STR		R2		R0
			ADI		R0		-4
			STR		R2		R0
			ADI		R0		-4
			STR		R2		R0
			MOV		R4		SL
			CMP		R4		R0
			BGT		R4		OVERFLOW
			MOV		SP		R0
			MOV		R4		PC
			ADI		R4		24
			STR		R4		R1
			JMP		reset					# calling reset(1, 0, 0, 0);
			MOV		SP		FP				# De-Allocating
			MOV		R5		SP
			CMP		R5		SB
			BGT		R5		UNDERFLOW
			LDR		R5		FP
			ADI		R5		-4
			LDR		FP		R5
											# then finally call getdata();
			MOV		R0		SP				# getdata();
			MOV		R1		R0
			ADI		R0		-4
			MOV		FP		R0
			ADI		R0		-4
			MOV		R4		SL
			CMP		R4		R0
			BGT		R4		OVERFLOW
			MOV		SP		R0
			MOV		R4		PC
			ADI		R4		24
			STR		R4		R1
			JMP		getdata					# calling getdata();
			MOV		SP		FP				# De-Allocation
			MOV		R5		SP
			CMP		R5		SB
			BGT		R5		UNDERFLOW
			LDR		R5		FP
			ADI		R5		-4
			LDR		FP		R5

											# end of while1

end_of_main	TRP		0						# end of main



		

OVERFLOW	LDB		R3		newline
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
	#		TRP		0

UNDERFLOW	LDB		R3		newline
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
	#		TRP		0


uu			.BYT	'u'
nn			.BYT	'n'
dd			.BYT	'd'
ee			.BYT	'e'
rr			.BYT	'r'
ff			.BYT	'f'
ll			.BYT	'l'
oo			.BYT	'o'
ww			.BYT	'w'
vv			.BYT	'v'

ii			.BYT	'i'
ss			.BYT	's'
tt			.BYT	't'
aa			.BYT	'a'
mm			.BYT	'm'
bb			.BYT	'b'
NN			.BYT	'N'
BB			.BYT	'B'
OO			.BYT	'O'
pp			.BYT	'p'

gg			.BYT	'g'
stop_symbol	.BYT	'@'
positive	.BYT	'+'
negative	.BYT	'-'
question	.BYT	'?'
exclaim		.BYT	'!'
newline		.BYT	'\n'
space		.BYT	' '



#	Global variables:

zero		.INT	0
SIZE		.INT	7
cnt			.INT
tenth		.INT
c			.BYT	#'t'
			.BYT	#'e'
			.BYT	#'s'
			.BYT	#'t'
			.BYT	#'i'
			.BYT	#'n'
			.BYT	#'g'
data		.INT
flag		.INT
opdv		.INT						# opd()'s accumulator




