

#Find Main() and start there.
		JMP		Main












#opd		SUB		R1		R1
		
opd		MOV		R0		SP		# getting local variables
		SUB		R1		R1
		STR		R1		R0
		ADI		SP		-4

		ADI		R0		12		# 
		LDR		R1		R0

		ADI		R0		4		# c[0]
		LDR		R2		R0
		ADI		R0		4		# tenth
		LDR		R4		R0
		ADI		R0		4		# c[cnt]
		








#############################
#	void flush() function	#
#############################

flush	SUB		R1		R1
		STR		R1		data
		TRP		4
		STB		R3		c
floop	SUB		R2		R2
		STB		R2		newLine
		CMP		R2		R3
		BRZ		R2		~flush
		TRP		4
		STB		R3		c
		JMP		floop
~flush	MOV		R4		FP		# Return to previous function
		ADI		R4		4
		LDR		PC		R4
#		TRP		0

#################################
#	void getdata()	function	#
#################################

getdata	LDR		R1		cnt
		LDR		R2		SIZE
		CMP		R1		R2
		BLT		R1		getchar
		JMP		toobig
getchar	LDA		R4		c
		LDR		R1		cnt
		ADD		R4		R1
		TRP		4
		STB		R4		R3
		ADI		R1		1
		STR		R1		cnt
		JMP		~getdata
toobig	LDB		R3		newLine
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
		LDB		R3		newLine
		TRP		3
								# activation record for flush();
		MOV		R0		SP		# flush();
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
		JMP		flush			# calling flush();
		MOV		SP		FP
		ADI		SP		4
		SUB		FP		FP
		ADI		FP		-1


~getdata	MOV		R4		FP	# returning to previous function
		ADI		R4		4
		LDR		PC		R4
	#	TRP		0







reset	MOV		R0		SP		# getting local variables
		SUB		R1		R1
		STR		R1		R0
		ADI		SP		-4
		LDA		R2		c		# sets all values in c[] to 0.
		SUB		R1		R1
		LDR		R4		SIZE
		CMP		R1		R4
		BGT		R1		reafter
reloop	SUB		R7		R7
		LDA		R2		c
		LDR		R1		cnt
		SUB		R2		R1
		STB		R2		R7
		ADI		R1		1
		STR		R1		cnt
		LDR		R5		SIZE
		CMP		R5		R1
		BLT		R5		reloop
reafter MOV		R0		SP		
		ADI		R0		12
		LDR		R1		R0
		ADI		R0		4
		LDR		R2		R0
		ADI		R0		4
		LDR		R3		R0
		ADI		R0		4
		LDR		R4		R0
		ADI		R0		4
		STR		R1		data	# setting data = w
		STR		R2		opdv	# setting opdv = x
		STR		R3		cnt		# setting cnt = y
		STR		R4		flag	# setting flag = z
~reset	MOV		R5		FP		# return to previous function
		ADI		R5		-4
		MOV		R4		R5
		CMP		R4		SB
		BGT		R4		UNDERFLOW
		LDR		PC		R5

#################################################


Main	MOV		R0		SP		# creating activation records...
		MOV		R1		R0		# reset(1, 0, 0, 0);
		ADI		R0		-4
		MOV		FP		R0
		ADI		R0		-4
		SUB		R2		R2
		ADI		R2		1
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
		ADI		R4		48
		STR		R4		FP
		JMP		reset			# calling reset(1, 0, 0, 0);

		TRP		99
		MOV		R0		SP		# getdata();
		MOV		R1		R0
		ADI		R0		-4
		MOV		FP		R0
		ADI		R0		-4
		MOV		R4		SL
		CMP		R4		R0
		BGT		R4		OVERFLOW
		MOV		SP		R0
		MOV		R4		PC
		ADI		R4		48
		STR		R4		R4
		JMP		getdata			# calling getdata();

		TRP		99
		MOV		SP		FP		# De-Allocation
		MOV		R5		SP
		CMP		R5		SB
		BGT		R5		UNDERFLOW
		LDR		R5		FP
		ADI		R5		-4
		LDR		FP		R5
#		ADI		SP		4
	#	SUB		FP		FP
#		ADI		FP		-1
		
		
#		MOV		R0		SP		# flush();
#		MOV		R1		R0
#		ADI		R0		-4
#		MOV		FP		R0
#		ADI		R0		-4
#		MOV		R4		SL
#		CMP		R4		R0
#		BGT		R4		OVERFLOW
#		MOV		SP		R0
#		MOV		R4		PC
#		ADI		R4		24
#		STR		R4		R1
#		JMP		flush			# calling flush();
#		MOV		SP		FP
#		ADI		SP		4
#		SUB		FP		FP
#		ADI		FP		-1

while1	LDA		R6		c		# while c[0] != '@'
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
										#if true:
getnext	MOV		R0		SP		# create activation record for getdata();
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
		JMP		getdata			# calling getdata();
		MOV		SP		FP		# De-Allocation
		MOV		R5		SP
		CMP		R5		SB
		BGT		R5		UNDERFLOW
		LDR		R5		FP
		ADI		R5		-4
		LDR		FP		R5

									#if false:
getsign	LDA		R6		c
		LDA		R7		c
		ADI		R7		1
		STB		R7		R6
		STB		R6		positive
		LDR		R5		cnt
		ADI		R5		1
		STR		R5		cnt

while2	LDR		R7		data
		BRZ		R7		end_of_main
		LDA		R6		c				# if c[cnt - 1] == '\n'
		LDR		R5		cnt	
		ADI		R5		-1
		ADD		R6		R5
		LDR		R4		newLine
		CMP		R6		R4
		BNZ		R6		while2else
		SUB		R7		R7
		STR		R7		data
		ADI		R7		1
		STR		R7		tenth
		LDR		R7		cnt
		ADI		R7		-2
		STR		R7		cnt

while3	LDR		R7		flag
		BNZ		R7		while3break
		LDR		R6		cnt
		BRZ		R6		while3break
										# call opd(c[0], tenth, c[cnt]);
		TRP		99

		MOV		R0		SP				# making activation record for opd();
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
		ADI		R4		20		# might be 48?
		STR		R4		R1
		JMP		opd				# calling opd
		MOV		SP		FP
		MOV		R5		SP
		CMP		R5		SB
		BGT		R5		UNDERFLOW
		LDR		R5		FP
		ADI		R5		-4
		LDR		FP		R5


										# end of while3

while3break		LDR		R7		flag
		BRZ		R7		while2else			
			LDB		R3		OO			# printf "Operand is %d\n", opdv
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
			LDB		R3		newLine
			TRP		3					

while2else		MOV		R0		SP		# create activation record for getdata();
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
		JMP		getdata			# calling getdata();
		MOV		SP		FP		# De-Allocation
		MOV		R5		SP
		CMP		R5		SB
		BGT		R5		UNDERFLOW
		LDR		R5		FP
		ADI		R5		-4
		LDR		FP		R5
								# end of while2

		MOV		R0		SP		# creating activation records...
		MOV		R1		R0		# reset(1, 0, 0, 0);
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
		JMP		reset			# calling reset(1, 0, 0, 0);
		MOV		SP		FP		# De-Allocating
		MOV		R5		SP
		CMP		R5		SB
		BGT		R5		UNDERFLOW
		LDR		R5		FP
		ADI		R5		-4
		LDR		FP		R5
								# then finally call getdata();
		MOV		R0		SP		# getdata();
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
		JMP		getdata			# calling getdata();
		MOV		SP		FP		# De-Allocation
		MOV		R5		SP
		CMP		R5		SB
		BGT		R5		UNDERFLOW
		LDR		R5		FP
		ADI		R5		-4
		LDR		FP		R5

								# end of while1

end_of_main		TRP		0				# end of main



		

OVERFLOW	LDB		R3		newLine
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
			LDB		R3		newLine
			TRP		3
			TRP		3
	#		TRP		0

UNDERFLOW	LDB		R3		newLine
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
			LDB		R3		newLine
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
newLine		.BYT	'\n'
space		.BYT	' '



#	Global variables:

zero		.INT	0
SIZE		.INT	7
cnt			.INT
tenth		.INT
c			.BYT 
			.BYT
			.BYT
			.BYT
			.BYT 
			.BYT 
			.BYT 
data		.INT
flag		.INT
opdv		.INT				# opd()'s accumulator




