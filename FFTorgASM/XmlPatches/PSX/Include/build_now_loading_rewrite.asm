#   ROUTINE: Build Now Loading Message (Rewrite)
build_now_loading_message:
        addiu   sp, sp, -48
        sw      s0, 16(sp)
        sw      s1, 20(sp)
        sw      s2, 24(sp)
        sw      s3, 28(sp)
        sw      s4, 32(sp)
        sw      s5, 36(sp)
        sw      ra, 40(sp)
    
        move    s1, a1
        move    s2, a2
    
        lui     s3, 0x8005
        addiu   s0, s3, -0x28e0
        sw      a0, 0x8004760c
        move    a0, s0
        
        li      v0, 9
        sb      v0, -0x28dd(s3)
        li      v0, 0x2c
        sb      v0, -0x28d9(s3)
        
        li      t0, 0x80
        li      t1, 0x80
        li      t2, 0x80
        sw      zero, -0x1a48(s3)
        sb      t0, -0x28dc(s3)
        sb      t1, -0x28db(s3)
        sb      t2, -0x28da(s3)
    
        jal     0x80023c68
        li      a1, 0
    
        li      v0, 0x1f
        sh      v0, -0x28ca(s3)
        li      v0, 0x7887
        sh      v0, -0x28d2(s3)
        
        li      s5, 1
        addiu   s4, s0, 40
        
    copy_loop:
        move    a0, s4
        move    a1, s0
        jal     copy_words
        li      a2, 40
        addiu   s5, s5, 1
        sltiu   t0, s5, 7
        bne     t0, zero, copy_loop
        addiu   s4, s4, 40
    
        #   "No" block
        la      a0, 0x8004d720
        addiu   a1, s2, 200
        addiu   a2, s1, 160
        addiu   a3, s2, 208
        addiu   t0, s1, 172
        li      t1, 155
        li      t2, 228
        li      t3, 163
        jal     write_and_copy_block
        li      t4, 240
        
        #   "w" block
        la      a0, 0x8004d748
        addiu   a1, s2, 200
        addiu   a2, s1, 171
        addiu   a3, s2, 208
        addiu   t0, s1, 178
        li      t1, 163
        li      t2, 228
        li      t3, 171
        jal     write_and_copy_block
        li      t4, 235
        
        #   "loa" block
        la      a0, 0x8004d770
        addiu   a1, s2, 200
        addiu   a2, s1, 183
        addiu   a3, s2, 208
        addiu   t0, s1, 195
        li      t1, 85
        li      t2, 173
        li      t3, 93
        jal     write_and_copy_block
        li      t4, 185
    
        #   "d" block
        la      a0, 0x8004d798
        addiu   a1, s2, 200
        addiu   a2, s1, 195
        addiu   a3, s2, 208
        addiu   t0, s1, 200
        li      t1, 96
        li      t2, 209
        li      t3, 104
        jal     write_and_copy_block
        li      t4, 214
        
        #   "i" block
        la      a0, 0x8004d7c0
        addiu   a1, s2, 200
        addiu   a2, s1, 199
        addiu   a3, s2, 208
        addiu   t0, s1, 202
        li      t1, 82
        li      t2, 124
        li      t3, 90
        jal     write_and_copy_block
        li      t4, 127
        
        #   "n" block
        la      a0, 0x8004d7e8
        addiu   a1, s2, 200
        addiu   a2, s1, 201
        addiu   a3, s2, 208
        addiu   t0, s1, 206
        li      t1, 204
        li      t2, 206
        li      t3, 212
        jal     write_and_copy_block
        li      t4, 211
    
        #   "g" block
        la      a0, 0x8004d810
        addiu   a1, s2, 202
        addiu   a2, s1, 205
        addiu   a3, s2, 210
        addiu   t0, s1, 210
        li      t1, 164
        li      t2, 235
        li      t3, 172
        jal     write_and_copy_block
        li      t4, 240
    
        la      a0, 0x800459e0
        li      a1, 0x70
        jal     0x80022d78
        li      a2, 0x1e2
    
        lw      ra, 40(sp)
        lw      s5, 36(sp)
        lw      s4, 32(sp)
        lw      s3, 28(sp)
        lw      s2, 24(sp)
        lw      s1, 20(sp)
        lw      s0, 16(sp)
        jr      ra
        addiu   sp, sp, 48



#   ROUTINE: Write and copy block
#       Combines write block with copy segment.
#       Parameters: Same as Write Block
write_and_copy_block:
        addiu   sp, sp, -16
        sw      s0, 4(sp)
        sw      ra, 8(sp)
        
        jal     write_block
        move    s0, a0
        
        addiu   a0, s0, 0x118
        move    a1, s0
        jal     copy_words
        li      a2, 40
        
        lw      ra, 8(sp)
        lw      s0, 4(sp)
        jr      ra
        addiu   sp, sp, 16
        

        
#   ROUTINE: Write block
#       Writes a block corresponding to a graphic.
#       Parameters:
#           a0 = Base pointer for block
#           a1 = Top Y Position (Screen)
#           a2 = Left X Position (Screen)
#           a3 = Bottom Y Position (Screen)
#           t0 = Right X Position (Screen)
#           t1 = Top Y Position (Bitmap)
#           t2 = Left X Position (Bitmap)
#           t3 = Bottom Y Position (Bitmap)
#           t4 = Right X Position (Bitmap)
write_block:
        sh      a1, 10(a0)
        sh      a1, 18(a0)
        sh      a2, 8(a0)
        sh      a2, 24(a0)
        sh      a3, 26(a0)
        sh      a3, 34(a0)
        sh      t0, 16(a0)
        sh      t0, 32(a0)
        sb      t1, 13(a0)
        sb      t1, 21(a0)
        sb      t2, 12(a0)
        sb      t4, 20(a0)
        sb      t2, 28(a0)
        sb      t3, 29(a0)
        sb      t4, 36(a0)
        jr      ra
        sb      t3, 37(a0)

        
        
#   ROUTINE: Copy words
#       Word-aligned memcpy.  Size must be multiple of 4.
#       Parameters:
#           a0 = dest, a1 = src, a2 = size
copy_words:
        beq     a2, zero, copy_words_end
        nop
        lw      t0, 0(a1)
        addiu   a2, a2, -4
        sw      t0, 0(a0)
        addiu   a0, a0, 4
        j       copy_words
        addiu   a1, a1, 4
    copy_words_end:
        jr      ra
        nop
