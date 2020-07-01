.label  @get_party_data_pointer, 0x80059af0
.label  @find_free_party_index, 0x80059d5c
.label  @calculate_unlocked_jobs, 0x8005dc14
.label  @store_three_bytes, 0x8005ded8
.label  @copy_bytes, 0x8005e254
.label  @get_total_equipment_quantity, 0x8005e288
.label  @calculate_zodiac_symbol, 0x8005e5d8

.label  @world_find_text_entry, 0x800e6edc
.label  @world_display_menu_text_entry, 0x800fe774
.label  @world_display_specific_menu_text, 0x800fe7d4
.label  @generate_formation_unit_name_string, 0x80108920
.label  @world_obtain_gil, 0x801207bc
.label  @world_dismiss_unit, 0x80120930
.label  @world_run_menu_thread, 0x8012ab78

.label  @address_initial_bench_location, 0x800579c8
.label  @address_party_unit_data, 0x80057f74
.label  @address_item_quantities_array, 0x800596e0
.label  @address_item_data_price, 0x80062ec0
.label  @address_job_level_jp_req_table, 0x80066182
.label  @address_selected_formation_unit_index, 0x8018ba20
.label  @address_formation_show_unit_info_panel, 0x8018ba25
.label  @address_sound_effect_id, 0x8018bacc
.label  @address_formation_show_unit_info_panel_stored_value, 0x8018bae9
.label  @address_formation_unit_table, 0x801cd5ec

.eqv    %size_party_unit, 256
.eqv    %size_bench_unit, 84


#   ROUTINE: Handle unit bench
#       Parameters:
#           a0 = Formation unit index
#           a1 = Bench slot index

@handle_unit_bench:

        addiu   sp, sp, -32
        sw      s0, 4(sp)
        sw      s1, 8(sp)
        #sw      s2, 12(sp)
        #sw      s3, 16(sp)
        sw      ra, 20(sp)
        
        move    s0, a0                      #   formationIndex
        
        sb      zero, @address_restore_unit_array (a1)
        
        jal     @get_bench_slot_pointer
        move    a0, a1
        
        sll     t2, s0, 2
        lw      t2, @address_formation_unit_table (t2)
        nop
        
        lhu     a0, 0x2c(t2)                #   unitIndex
        jal     @get_party_data_pointer
        move    s1, v0                      #   benchSlot
        
        lbu     t0, 0(s1)
        nop
        beq     t0, zero, handle_unit_bench_default
        move    a0, v0
        
        jal     @obtain_party_unit_items
        move    s0, a0
        
        move    a0, s0
        jal     @swap_unit_to_bench
        move    a1, s1
        
        jal     @generate_formation_unit_name_string
        nop
        
        j       handle_unit_bench_end
        nop
        
    handle_unit_bench_default:
        jal     @bench_unit
        move    a1, s1
        
        jal     @world_dismiss_unit
        move    a0, s0
        
    handle_unit_bench_end:
        lw      ra, 20(sp)
        #lw      s3, 16(sp)
        #lw      s2, 12(sp)
        lw      s1, 8(sp)
        lw      s0, 4(sp)
        jr      ra
        addiu   sp, sp, 32

        
#   ROUTINE: Handle unit unbench
#       Parameters:
#           a0 = Formation unit index
#           a1 = Bench slot index

@handle_unit_unbench:

        addiu   sp, sp, -32
        sw      s0, 4(sp)
        sw      s1, 8(sp)
        sw      s2, 12(sp)
        #sw      s3, 16(sp)
        sw      ra, 20(sp)
        
        move    s0, a0                      #   formationIndex
        
        jal     @get_bench_slot_pointer
        move    a0, a1
        
        sll     t2, s0, 2
        lw      t2, @address_formation_unit_table (t2)
        li      t3, 0xff
        
        #   Returns v0 = partyUnit, v1 = partyIndex
        lhu     s2, 0x2c(t2)                #   unitIndex
        move    s1, v0                      #   benchSlot
        jal     @get_first_empty_active_party_slot
        move    a0, s2
        
        lbu     t0, 1(v0)
        move    a2, v1                      #   partyIndex
        beq     t0, t3, handle_unit_unbench_default
        move    a0, v0                      #   partyUnit
        
        jal     @get_party_data_pointer
        move    a0, s2
        move    s0, v0
        
        jal     @obtain_party_unit_items
        move    a0, s0
        
        move    a0, s0
        jal     @swap_unit_to_bench
        move    a1, s1
        
        j       handle_unit_unbench_post_action
        nop
        
    handle_unit_unbench_default:
        jal     @unbench_unit
        move    a1, s1
        
        #   Empty bench slot
        sb      $0, 0(s1)
    
    handle_unit_unbench_post_action:
        jal     @generate_formation_unit_name_string
        nop
        
    handle_unit_unbench_end:
        lw      ra, 20(sp)
        #lw      s3, 16(sp)
        lw      s2, 12(sp)
        lw      s1, 8(sp)
        lw      s0, 4(sp)
        jr      ra
        addiu   sp, sp, 32
        
        
#   ROUTINE: Swap unit to bench
#       Parameters:
#           a0 = Party unit data pointer
#           a1 = Bench slot pointer       

@swap_unit_to_bench:

        addiu   sp, sp, -104
        sw      s0, 4(sp)
        sw      s1, 8(sp)
        sw      ra, 12(sp)

        move    s0, a0
        move    s1, a1
        
        #   Bench from active party to stack
        jal     @bench_unit
        addiu   a1, sp, 16
        
        #   Unbench from bench to active party 
        lbu     a2, 1(s0)
        move    a0, s0
        jal     @unbench_unit
        move    a1, s1
        
        #   Copy from stack to bench
        addiu   a0, sp, 16
        move    a1, s1
        jal     @copy_bytes
        li      a2, %size_bench_unit
        
        lw      ra, 12(sp)
        lw      s1, 8(sp)
        lw      s0, 4(sp)
        jr      ra
        addiu   sp, sp, 104
        

#   ROUTINE: Bench unit
#       Parameters:
#           a0 = Party unit data pointer
#           a1 = Bench slot pointer

@bench_unit:

        addiu   sp, sp, -24
        sw      s0, 4(sp)
        sw      s1, 8(sp)
        sw      ra, 12(sp)

        move    s0, a0
        move    s1, a1
        
        #   Base Class, Job, Flags, Name ID
        lbu     t0, 0(s0)
        lbu     t1, 2(s0)
        lbu     t2, 4(s0)
        lhu     t3, 0xce(s0)
        sb      t0, 0(s1)
        sb      t1, 1(s1)
        sb      t2, 2(s1)
        sb      t3, 83(s1)
        
        #   Birthday, Level
        lbu     t0, 6(s0)
        lbu     t1, 5(s0)
        lbu     t2, 0x16(s0)
        andi    t0, t0, 1
        sll     t0, t0, 7
        sb      t1, 3(s1)
        or      t0, t0, t2
        sb      t0, 4(s1)
        
        #   Name ID (high byte) bits...
        srl     t4, t3, 2
        andi    t4, t4, 0x80
        srl     t5, t3, 1
        andi    t5, t5, 0x80
        
        #   Brave, Faith (with Name ID bits)
        lbu     t0, 0x17(s0)
        lbu     t1, 0x18(s0)
        or      t0, t0, t4
        or      t1, t1, t5
        sb      t0, 5(s1)
        sb      t1, 6(s1)
        
        #   Raw stats
        addiu   a0, s0, 0x19
        addiu   a1, s1, 7
        jal     @copy_bytes
        li      a2, 15

        #   Learned abilities, R/S/M, job levels
        li      t0, 0       #   Job index (0 to 19)
        li      a0, 0       #   Current value for R/S/M
        li      a1, 0       #   Current value for Job Levels
    bench_unit_job_loop:
        sll     t2, t0, 1
        addu    t4, s1, t2
        addu    t2, t2, t0
        addu    t3, s0, t2
        
        #   Skip to R/S/M check, if (index == 19)
        li      t5, 19
        beq     t0, t5, bench_unit_job_loop_rsm_check
        sll     a0, a0, 6
        
        #   Action abilities
        lbu     t5, 0x2b(t3)
        lbu     t6, 0x2c(t3)
        sb      t5, 22(t4)
        sb      t6, 23(t4)
    
        #   Load R/S/M and add to current value
        lbu     t5, 0x2d(t3)
        nop
        srl     t5, t5, 2
        or      a0, a0, t5
        
    bench_unit_job_loop_rsm_check:
        #   R/S/M - Check if ((index + 1) % 4) == 0, and if not, skip to next section (Job Levels)
        addiu   t6, t0, 1
        andi    t7, t6, 0x03
        bne     t7, zero, bench_unit_job_loop_past_rsm
        nop
        
        #   R/S/M - (write index) = (((index + 1) / 4) - 1) * 3
        srl     t7, t6, 2
        addiu   t7, t7, -1
        sll     t8, t7, 1
        addu    t7, t8, t7
        
        #   R/S/M - Write bytes from current value.  Lower order bits represent newer (higher index) values.
        addu    t7, t7, s1
        sb      a0, 62(t7)
        srl     a0, a0, 8
        sb      a0, 61(t7)
        srl     a0, a0, 8
        sb      a0, 60(t7)
        
        #   R/S/M - Reset current value to 0
        li      a0, 0
        
    bench_unit_job_loop_past_rsm:   
        #   Job Levels        
        andi    t7, t0, 1
        bne     t7, zero, bench_unit_job_loop_past_job_level_load
        andi    t5, t9, 0x0f
        
        srl     t3, t0, 1
        addu    t3, s0, t3
        lbu     t9, 0x64(t3)        #   Loaded job level value
        nop
        srl     t5, t9, 4
        
    bench_unit_job_loop_past_job_level_load:
        sltiu   t7, t5, 1
        addu    t5, t5, t7          #   If value == 0, set value = 1
        addiu   t5, t5, -1          #   (1 to 8) -> (0 to 7)
        
        sll     a1, a1, 3
        or      a1, a1, t5          #   Add to current value
        
        #   Job levels - Check if ((index + 1) % 8) == 0, and if not, skip to next section
        addiu   t6, t0, 1
        andi    t7, t6, 0x07
        bne     t7, zero, bench_unit_job_loop_past_job_levels
        nop
        
        #   Job levels - (write index) = (((index + 1) / 8) - 1) * 3
        srl     t7, t6, 3
        addiu   t7, t7, -1
        sll     t8, t7, 1
        addu    t7, t8, t7
        
        #   Job levels - Write bytes from current value.  Lower order bits represent newer (higher index) values.
        addu    t7, t7, s1
        sb      a1, 77(t7)
        srl     a1, a1, 8
        sb      a1, 76(t7)
        srl     a1, a1, 8
        sb      a1, 75(t7)
        
        #   Job levels - Reset current value to 0
        li      a1, 0
        
    bench_unit_job_loop_past_job_levels:
        #   Bottom of loop - check condition
        sltiu   t1, t0, 19
        bne     t1, zero, bench_unit_job_loop
        addiu   t0, t0, 1
        
        #   Write remaining job levels
        sll     t0, a1, 4
        sb      t0, 82(s1)
        srl     t0, t0, 8
        sb      t0, 81(s1)
        
        #   Return!
        lw      ra, 12(sp)
        lw      s1, 8(sp)
        lw      s0, 4(sp)
        jr      ra
        addiu   sp, sp, 24

        
#   ROUTINE: Unbench unit
#       Parameters:
#           a0 = Party unit data pointer
#           a1 = Bench slot pointer
#           a2 = Party index

@unbench_unit:
        
        addiu   sp, sp, -32
        sw      s0, 4(sp)
        sw      s1, 8(sp)
        sw      s2, 12(sp)
        sw      ra, 16(sp)
        
        move    s0, a0
        move    s1, a1
        
        #   Base Class, Party index, Job, Team, Flags
        lbu     t0, 0(s1)
        lbu     t1, 1(s1)
        lbu     t2, 2(s1)
        sb      t0, 0(s0)
        sb      a2, 1(s0)
        sb      t1, 2(s0)
        sb      $0, 3(s0)
        sb      t2, 4(s0)
        
        #   Birthday, Level
        lbu     t2, 3(s1)
        lbu     t1, 4(s1)
        sb      t2, 5(s0)
        andi    t0, t1, 0x7f
        sb      t0, 22(s0)
        andi    s2, t1, 0x80
        sll     a0, s2, 1
        jal     @calculate_zodiac_symbol
        or      a0, a0, t2
        
        sll     t0, v0, 4
        srl     t1, s2, 7
        or      t0, t0, t1
        sb      t0, 6(s0)
        
        sb      $0, 7(s0)           #   Secondary Skillset
        sw      $0, 8(s0)           #   Reaction Ability, Support Ability
        sh      $0, 12(s0)          #   Movement Ability
        
        li      t0, -1
        sh      t0, 14(s0)          #   Head, Body
        sw      t0, 16(s0)          #   Accessory, Right Hand Weapon, Right Hand Shield, Left Hand Weapon
        sb      t0, 20(s0)          #   Left Hand Shield
        
        sb      $0, 21(s0)          #   Experience
        
        #   Brave, Faith, Name
        lbu     t0, 5(s1)
        lbu     t1, 6(s1)
        lbu     t2, 83(s1)
        
        #   Name ID
        andi    t3, t0, 0x80
        sll     t3, t3, 2
        andi    t4, t1, 0x80
        sll     t4, t4, 1
        or      t3, t3, t4
        or      v0, t3, t2
        sh      v0, 0xce(s0)
        
        #   Brave
        andi    t0, t0, 0x7f
        sb      t0, 23(s0)
        
        #   Faith
        andi    t1, t1, 0x7f
        sb      t1, 24(s0)
        
        #   Name
        #jal     @get_bench_unit_name_lookup_value
        #move    a0, s1
        
        jal     @world_find_text_entry
        ori     a0, v0, 0x4800
        
        move    a0, v0
        addiu   a1, s0, 0xbe
        jal     @copy_bytes
        li      a2, 16
        
        #   Raw Stats
        addiu   a0, s1, 7
        addiu   a1, s0, 25
        jal     @copy_bytes
        li      a2, 15
        
        #   Learned abilities, R/S/M, job levels
        li      t0, 0       #   Job index (0 to 19)
        #li      a0, 0       #   Current value for R/S/M
        #li      a1, 0       #   Current value for Job Levels
    unbench_unit_job_loop:
        sll     t2, t0, 1
        addu    t5, t2, t0
        addu    t3, s0, t5
        #addu    t4, s1, t5
        addu    t4, s1, t2
        
        #   JP
        addu    v1, s0, t2
        sh      $0, 0x6e(v1)
        
        #   Skip Action abilities if (index == 19)
        li      t5, 19
        beq     t0, t5, unbench_unit_job_loop_past_action_abilities
        nop
        
        #   Action abilities        
        lbu     t5, 22(t4)
        lbu     t6, 23(t4)
        sb      t5, 0x2b(t3)
        sb      t6, 0x2c(t3)
        
    unbench_unit_job_loop_past_action_abilities:
        #   R/S/M - Check if ((index + 1) % 4) == 0, and if not, skip to next section (Job Levels)
        addiu   t6, t0, 1
        andi    t7, t6, 0x03
        bne     t7, zero, unbench_unit_job_loop_past_rsm
        nop
        
        #   R/S/M - (read index) = (((index + 1) / 4) - 1) * 3
        srl     t7, t6, 2
        addiu   t7, t7, -1
        sll     t8, t7, 1
        addu    t7, t8, t7
        
        #   R/S/M - Load values
        addu    t7, t7, s1
        lbu     t4, 60(t7)
        lbu     t5, 61(t7)
        lbu     t6, 62(t7)
        
        #   R/S/M - Construct bytes
        #srl     a0, t4, 2
        andi    a0, t4, 0xfc
        
        andi    a1, t4, 0x03
        sll     a1, a1, 4
        srl     t9, t5, 4
        or      a1, a1, t9
        sll     a1, a1, 2
        
        andi    a2, t5, 0x0f
        sll     a2, a2, 2
        srl     t9, t6, 6
        or      a2, a2, t9
        sll     a2, a2, 2
        
        andi    a3, t6, 0x3f
        sll     a3, a3, 2
        
        #   R/S/M - Store bytes
        sb      a0, 0x24(t3)
        sb      a1, 0x27(t3)
        sb      a2, 0x2a(t3)
        
        #   Skip storing final byte if (index == 19)
        li      t5, 19
        beq     t0, t5, unbench_unit_job_loop_past_rsm
        nop
        
        sb      a3, 0x2d(t3)
        
    unbench_unit_job_loop_past_rsm:
        #   Job levels - Check if ((index + 1) % 8) == 0, and if not, skip to next section
        addiu   t6, t0, 1
        andi    t7, t6, 0x07
        srl     t3, t6, 1
        addiu   t3, t3, -1
        bne     t7, zero, unbench_unit_job_loop_past_job_levels
        addu    t3, s0, t3
        
        #   Job levels - (read index) = (((index + 1) / 8) - 1) * 3
        srl     t7, t6, 3
        addiu   t7, t7, -1
        sll     t8, t7, 1
        addu    t7, t8, t7
        
        #   Job levels - Load values
        addu    t7, t7, s1
        lbu     t4, 75(t7)
        lbu     t5, 76(t7)
        lbu     t6, 77(t7)
        
        #   Job levels - Calculate and write values
        srl     a0, t4, 5
        srl     t8, t4, 2
        andi    t8, t8, 0x07
        addiu   a0, a0, 1
        addiu   t8, t8, 1
        sll     a0, a0, 4
        or      a0, a0, t8
        sb      a0, 0x61(t3)
        
        andi    a1, t4, 0x03
        sll     a1, a1, 1
        srl     t8, t5, 7
        or      a1, a1, t8
        srl     t8, t5, 4
        andi    t8, t8, 0x07
        addiu   a1, a1, 1
        addiu   t8, t8, 1
        sll     a1, a1, 4
        or      a1, a1, t8
        sb      a1, 0x62(t3)
    
        srl     a2, t5, 1
        andi    a2, a2, 0x07
        andi    t8, t5, 1
        sll     t8, t8, 2
        srl     t9, t6, 6
        or      t8, t8, t9
        addiu   a2, a2, 1
        addiu   t8, t8, 1
        sll     a2, a2, 4
        or      a2, a2, t8
        sb      a2, 0x63(t3)
        
        srl     a3, t6, 3
        andi    a3, a3, 0x07
        andi    t8, t6, 0x07
        addiu   a3, a3, 1
        addiu   t8, t8, 1
        sll     a3, a3, 4
        or      a3, a3, t8
        sb      a3, 0x64(t3)
    
    unbench_unit_job_loop_past_job_levels:
        #   Bottom of loop - check condition
        sltiu   t1, t0, 19
        bne     t1, zero, unbench_unit_job_loop
        addiu   t0, t0, 1
    
        #   Write remaining job levels
        lbu     t0, 81(s1)
        lbu     t1, 82(s1)
        srl     t2, t0, 5
        srl     t3, t0, 2
        andi    t3, t3, 0x07
        addiu   t2, t2, 1
        addiu   t3, t3, 1
        sll     t2, t2, 4
        or      t2, t2, t3
        sb      t2, 0x6c(s0)
        
        andi    t2, t0, 0x03
        sll     t2, t2, 1
        srl     t8, t1, 7
        or      t2, t2, t8
        srl     t3, t1, 4
        andi    t3, t3, 0x07
        addiu   t2, t2, 1
        addiu   t3, t3, 1
        sll     t2, t2, 4
        or      t2, t2, t3
        sb      t2, 0x6d(s0)
        
        #   Total JP
        move    t4, s0
        addiu   t5, s0, 10
        move    t6, s0
    unbench_unit_total_jp_loop:    
        lbu     t0, 100(t4)
        lui     t3, %hi(@address_job_level_jp_req_table)
        srl     t1, t0, 4
        sll     t1, t1, 1
        andi    t2, t0, 0x0f
        sll     t2, t2, 1
        addu    t1, t1, t3
        addu    t2, t2, t3
        lhu     t1, %lo(@address_job_level_jp_req_table) (t1)
        lhu     t2, %lo(@address_job_level_jp_req_table) (t2)
        sh      t1, 0x96(t6)
        sh      t2, 0x98(t6)
        addiu   t4, t4, 1
        bne     t4, t5, unbench_unit_total_jp_loop
        addiu   t6, t6, 4
        
        #   Zero out Total JP for Bard/Dancer based on Male/Female
        lbu     t1, 4(s0)
        nop
        andi    t0, t1, 0x80
        beq     t0, zero, unbench_unit_bard_check
        andi    t0, t1, 0x40
        
        sh      zero, 0xba(s0)
        
    unbench_unit_bard_check:
        beq     t0, zero, unbench_unit_calc_unlocked_jobs
        nop
        
        sh      zero, 0xb8(s0)
        
    unbench_unit_calc_unlocked_jobs:
        #   Could call 0x5a8d4 (local, partyUnitIndex, 1) here if necessary to ensure all job data gets initialized... but the formation screen frame processing loop does it anyway.
        #   Unlocked Jobs
        lbu     a1, 4(s0)
        jal     @calculate_unlocked_jobs
        addiu   a0, s0, 100
        addiu   a0, s0, 40
        jal     @store_three_bytes
        move    a1, v0
       
        #   Zeroed out data : 0xd0 - 0xff
        addiu   t0, s0, 0xd0
        addiu   t1, s0, 0xfc 
    unbench_unit_zero_out_words_loop: 
        sw      $0, 0(t0)
        bne     t0, t1, unbench_unit_zero_out_words_loop
        addiu   t0, t0, 4
        
        #   Return!
        lw      ra, 16(sp)
        lw      s2, 12(sp)
        lw      s1, 8(sp)
        lw      s0, 4(sp)        
        jr      ra
        addiu   sp, sp, 32
        
        
#   ROUTINE: Get Bench Slot Pointer
#       Parameters:
#           a0 = Bench slot index
#       Returns:
#           v0 = Bench slot pointer

@get_bench_slot_pointer:

        lui     t1, %hi(@address_initial_bench_location)
        sltiu   t2, a0, 4
        bne     t2, zero, get_bench_slot_pointer_calc
        addiu   t1, t1, %lo(@address_initial_bench_location)
        
        la      t1, @extra_save_data_location
        addiu   a0, a0, -4
        
    get_bench_slot_pointer_calc:  
        sll     t0, a0, 2                   #   benchSlotIndex * 4
        addu    t0, t0, a0                  #   benchSlotIndex * 5
        sll     t0, t0, 2                   #   benchSlotIndex * 20
        addu    t0, t0, a0                  #   benchSlotIndex * 21
        sll     t0, t0, 2                   #   benchSlotIndex * 84
    
        jr      ra
        addu    v0, t1, t0
        

#   ROUTINE: Get first empty active party slot
#       Gets the first empty active party slot, starting from a specified index. Loops back around if necessary. Does not include guests.
#       Parameters:
#           a0 = Party index to start from
#       Returns:
#           v0 = Party unit data pointer of first unused active party slot, or null (0) if no empty slots exist
#           v1 = Party index of first unused active party slot, or -1 if no empty slots exist

@get_first_empty_active_party_slot:

        move    t2, a0
        li      t3, 0xff
        la      t4, @address_party_unit_data
        
    get_first_empty_active_party_slot_loop:        
        sll     t1, t2, 8
        addu    t1, t4, t1
        lbu     t0, 1(t1)
        move    v1, t2
        addiu   t2, t2, 1
        beq     t0, t3, get_first_empty_active_party_slot_end
        move    v0, t1
        
        andi    t2, t2, 0x0f
        li      v1, -1
        bne     t2, a0, get_first_empty_active_party_slot_loop
        li      v0, 0
        
    get_first_empty_active_party_slot_end:    
        jr      ra
        nop

        
#   ROUTINE: Get first empty bench slot
#       Gets the first empty bench slot pointer.
#       Returns:
#           v0 = Bench slot pointer of first unused bench slot, or null (0) if no empty slots exist.

@get_first_empty_bench_slot:

        addiu   sp, sp, -16
        sw      s0, 4(sp)
        sw      ra, 8(sp)
        
        li      s0, 0
        
    get_first_empty_bench_slot_loop:
        jal     @get_bench_slot_pointer
        move    a0, s0
        
        lbu     t0, 0(v0)
        nop
        beq     t0, zero, get_first_empty_bench_slot_end
        addiu   s0, s0, 1
        
        sltiu   t0, s0, 10
        bne     t0, zero, get_first_empty_bench_slot_loop
        li      v0, 0
        
    get_first_empty_bench_slot_end:
        lw      ra, 8(sp)
        lw      s0, 4(sp)
        jr      ra
        addiu   sp, sp, 16

        
#   ROUTINE: Is unit benchable?
#       Determines if a unit is benchable.
#       Benchable units are the same as the fieldable units in battle, with the exception of the first party slot (Ramza).
#       Parameters:
#           a0 = Party unit data pointer
#       Returns:
#           v0 = Benchable status of unit (1 = true, 0 = false)

@is_unit_benchable:

        lbu     t0, 0(a0)       #   Base class
        lbu     t1, 1(a0)       #   Party index
        lbu     t3, 4(a0)       #   Flags
        lbu     t2, 0xd0(a0)    #   Proposition status
        
        #   Not benchable, if:
        
        #   Ramza
        sltiu   t0, t0, 4
        bne     t0, zero, is_unit_benchable_end
        li      v0, 0

        #   Guest (or doesn't exist)
        sltiu   t0, t1, 16
        beq     t0, zero, is_unit_benchable_end
        li      v0, 0
        
        #   Is in first party slot  (Should be covered by Ramza check already, but for mods...?)
        beq     t1, zero, is_unit_benchable_end
        li      v0, 0
        
        #   On proposition
        bne     t2, zero, is_unit_benchable_end
        li      v0, 0
        
        #   Shrouded stats
        andi    t3, t3, 0x04
        bne     t3, zero, is_unit_benchable_end
        li      v0, 0
        
        #   Otherwise, benchable.
        li      v0, 1
        
    is_unit_benchable_end:
        jr      ra
        nop
        

#   ROUTINE: Generate Bench Unit Menu Data
#       Generates the bench unit menu string, and the list that indicates whether or not the entries are empty.
#       Parameters:
#           a0 = Bench menu status (1 = bench, 2 = unbench)
@generate_bench_unit_menu_data:

        addiu   sp, sp, -48
        sw      s0, 4(sp)
        sw      s1, 8(sp)
        sw      s2, 12(sp)
        sw      s3, 16(sp)
        sw      s4, 20(sp)
        sw      s5, 24(sp)
        sw      s6, 28(sp)
        sw      s7, 32(sp)
        sw      ra, 36(sp)
        
        li      s0, 0
        la      s1, @address_bench_menu_text
        li      s2, 0xfe
        li      s3, 0xf8
        la      s4, @address_bench_menu_entry_empty_text
        la      s5, @address_bench_menu_is_empty_entry
        move    s6, a0
        la      s7, @address_restore_unit_array
        
    generate_bench_unit_menu_data_bench_loop:
        jal     @get_bench_slot_pointer
        move    a0, s0
        
        li      t5, 0
        addu    t2, s5, s0
        sb      zero, 0(t2)
        move    t3, v0
        
        li      t4, 0
        addu    t6, s7, s0
        lbu     t6, 0(t6)
        
        lbu     t0, 0(v0)
        beq     v0, zero, generate_bench_unit_menu_data_bench_copy_loop
        move    v0, s4
        beq     t0, zero, generate_bench_unit_menu_data_bench_copy_loop
        nop
        
        li      t5, 1
        sb      t5, 0(t2)
        jal     @get_bench_unit_name_lookup_value
        move    a0, t3
        jal     @world_find_text_entry
        ori     a0, v0, 0x4800
        
        li      t4, 0
        addu    t6, s7, s0
        lbu     t6, 0(t6)
        
    generate_bench_unit_menu_data_bench_copy_loop:
        lbu     t0, 0(v0)
        addiu   v0, v0, 1
        
        #   Un-grey the color, if appropriate
        
        #   Check if this is the second character of the string
        xori    t2, t4, 1
        sltiu   t2, t2, 1
        
        #   Check if this is the Bench menu, or the slot is restorable
        
        #   Bench menu
        xori    t1, s6, 1
        sltiu   t1, t1, 1
        
        #   Restorable
        sltu    t6, zero, t6
        
        or      t1, t1, t6
        and     t2, t2, t1
        
        #   Check if this is an empty slot
        #xori    t1, t5, 0
        sltiu   t1, t5, 1
        and     t2, t2, t1
        
        #   Check if the old value is 4
        #xori    t1, t0, 4
        #sltiu   t1, t1, 1
        #and     t2, t2, t1
        
        beq     t2, zero, generate_bench_unit_menu_data_bench_copy_loop_past_color
        nop
        
        li      t0, 0
        
    generate_bench_unit_menu_data_bench_copy_loop_past_color:
        sb      t0, 0(s1)
        and     t2, t0, s2
        addiu   t4, t4, 1
        bne     t2, s2, generate_bench_unit_menu_data_bench_copy_loop
        addiu   s1, s1, 1
        
        addiu   s0, s0, 1
        sltiu   t0, s0, 10
        bne     t0, zero, generate_bench_unit_menu_data_bench_loop
        sb      s3, -1(s1)
        
        sb      s2, -1(s1)
        
        lw      ra, 36(sp)
        lw      s7, 32(sp)
        lw      s6, 28(sp)
        lw      s5, 24(sp)
        lw      s4, 20(sp)
        lw      s3, 16(sp)
        lw      s2, 12(sp)
        lw      s1, 8(sp)
        lw      s0, 4(sp)
        jr      ra
        addiu   sp, sp, 48
      
      
#   ROUTINE: Get bench unit name lookup value
#       Gets the lookup value (normally at 0xce of party unit data) for a bench unit.
#       Parameters:
#           a0 = Bench slot pointer
#       Returns:
#           v0 = Unit name lookup value

@get_bench_unit_name_lookup_value:

        #   Brave, Faith, Name
        lbu     t0, 5(a0)
        lbu     t1, 6(a0)
        lbu     t2, 83(a0)
        
        #   Name ID
        andi    t3, t0, 0x80
        sll     t3, t3, 2
        andi    t4, t1, 0x80
        sll     t4, t4, 1
        or      t3, t3, t4
        jr      ra
        or      v0, t3, t2

        #lbu     t2, 0(a0)
        #lbu     t0, 83(a0)
        #addiu   t3, t2, -0x80
        #sltiu   t3, t3, 3
        #sltu    t3, zero, t3
        #sll     t3, t3, 31
        #sra     t3, t3, 31
        #addiu   t1, t2, -0x7f
        #and     t1, t1, t3
        #sll     t2, t1, 8
        #jr      ra
        #or      v0, t2, t0
        
        
#   ROUTINE: Formation Screen Unit Menu Processing
@formation_screen_unit_menu_processing:

        addiu   sp, sp, -16
        sw      s0, 4(sp)
        sw      ra, 8(sp)
        
        lbu     t0, @address_unbench_inner_menu_status
        nop
        beq     t0, zero, formation_screen_unit_menu_processing_default
        nop
        
        jal     @formation_screen_unbench_inner_menu_processing
        nop
        j       formation_screen_unit_menu_processing_end
        nop
        
    formation_screen_unit_menu_processing_default:
        la      a1, @address_bench_menu_data
        jal     @world_run_menu_thread
        li      a0, 15
        
        sb      zero, @address_formation_show_unit_info_panel
        
        bne     v0, zero, formation_screen_unit_menu_processing_end
        li      t0, 1
        
        #   Selected entry / cancel processing
        lh      a1, @address_bench_menu_selection
        lui     t2, %hi(@address_bench_menu_status)
        lbu     t1, %lo(@address_bench_menu_status) (t2)
        nop
        #sb      zero, %lo(@address_bench_menu_status) (t2)
        #sb      t0, @address_formation_show_unit_info_panel
        
        #   If canceled, return
        bgez    a1, formation_screen_unit_menu_processing_past_cancel
        lui     t3, %hi(@address_sound_effect_id)
        
        j       formation_screen_unit_menu_processing_end
        sb      zero, %lo(@address_bench_menu_status) (t2)
    
    formation_screen_unit_menu_processing_past_cancel:
        #   Otherwise, handle action
        lbu     a0, @address_selected_formation_unit_index
        bne     t1, t0, formation_screen_unit_menu_processing_unbench
        sb      t0, %lo(@address_sound_effect_id) (t3)
        
        jal     @handle_unit_bench
        sb      zero, %lo(@address_bench_menu_status) (t2)
        j       formation_screen_unit_menu_processing_end
        nop
        
    formation_screen_unit_menu_processing_unbench:
        la      t0, @address_bench_menu_is_empty_entry
        addu    t0, t0, a1
        lbu     t0, 0(t0)
        nop
        beq     t0, zero, formation_screen_unit_menu_unbench_empty
        li      t1, 48
        
        #   Slot selected from Unbench menu
        li      t0, 1
        lui     at, %hi(@address_unbench_inner_menu_status)
        j       formation_screen_unit_menu_processing_end
        sb      t0, %lo(@address_unbench_inner_menu_status) (at)
        
    formation_screen_unit_menu_unbench_empty:
        lui     t4, %hi(@address_restore_unit_array)
        addu    t4, t4, a1
        lbu     t0, %lo(@address_restore_unit_array) (t4)
        sb      zero, %lo(@address_restore_unit_array) (t4)
        
        beq     t0, zero, formation_screen_unit_menu_processing_end
        sb      t1, %lo(@address_sound_effect_id) (t3)
        
        #   Handle Restore action
        li      t1, 1
        sb      t1, %lo(@address_sound_effect_id) (t3)
        #sb      zero, %lo(@address_bench_menu_status) (t2)
        
        move    s0, t0
        jal     @get_bench_slot_pointer
        move    a0, a1
        sb      s0, 0(v0)
        
        jal     @generate_bench_unit_menu_data
        li      a0, 2
    
    formation_screen_unit_menu_processing_end:
        lw      ra, 8(sp)
        lw      s0, 4(sp)
        jr      ra
        addiu   sp, sp, 16
 

#   ROUTINE: Find text entry (outer)
#       Parameters:
#           a0 = (Combined text index) of text to lookup
#       Returns:
#           v0 = Text pointer

@find_text_entry_outer:

        la      a3, @world_find_text_entry
        j       @handle_menu_text_entry_generic
        nop
 
 
#   ROUTINE: Display menu text entry (outer)
#       Parameters:
#           a0 = (Combined text index) of text to display
#           a1 = Pointer; Used as Parameter 2 in Routine 0x800ff284
#                Sometimes varies by menu, sometimes is the same
#           a2 = Menu data pointer?

@display_menu_text_entry_outer:

        la      a3, @world_display_menu_text_entry
        la      t0, @world_display_specific_menu_text
        j       @handle_menu_text_entry_generic
        nop

        
#   ROUTINE: Menu text entry generic
#       Handles a menu text entry.  
#       Either displays a text entry, or finds text.
#       Parameters:
#           a0 = (Combined text index) of text to display
#           a1 = Pointer; Used as Parameter 2 in Routine 0x800ff284
#                Sometimes varies by menu, sometimes is the same
#           a2 = Menu data pointer?
#           a3 = Function pointer (default)
#           t0 = Function pointer (specific)
#       Returns:
#           v0 = Function result

@handle_menu_text_entry_generic:
        
        addiu   sp, sp, -16
        sw      ra, 4(sp)
        
        andi    t1, a0, 0xffff
        li      t2, 0xd800
        beq     t1, t2, handle_menu_text_entry_generic_triangle_menu
        li      t2, 0xc880
        beq     t1, t2, handle_menu_text_entry_generic_bench_menu
        li      t2, 0xc881
        beq     t1, t2, handle_menu_text_entry_generic_unbench_inner_menu
        li      t2, 0xc882
        beq     t1, t2, handle_menu_text_entry_generic_bench_unit_select_text
        li      t2, 0xc883
        beq     t1, t2, handle_menu_text_entry_generic_unbench_unit_select_text
        li      t2, 0x0708
        beq     t1, t2, handle_menu_text_entry_generic_bench_menu_select_text
        nop
        j       handle_menu_text_entry_generic_default
        nop
     
    handle_menu_text_entry_generic_default:
        jalr    a3
        nop
        j       handle_menu_text_entry_generic_end
        nop
      
    handle_menu_text_entry_generic_triangle_menu:
        lui     t2, %hi(@address_triangle_menu_text)
        j       handle_menu_text_entry_generic_specific
        addiu   t2, t2, %lo(@address_triangle_menu_text)
    
    handle_menu_text_entry_generic_bench_menu:
        lui     t2, %hi(@address_bench_menu_text)
        j       handle_menu_text_entry_generic_specific
        addiu   t2, t2, %lo(@address_bench_menu_text)
        
    handle_menu_text_entry_generic_unbench_inner_menu:
        lui     t2, %hi(@address_unbench_inner_menu_text)
        j       handle_menu_text_entry_generic_specific
        addiu   t2, t2, %lo(@address_unbench_inner_menu_text)
        
    handle_menu_text_entry_generic_bench_unit_select_text:
        lui     t2, %hi(@address_bench_unit_select_text)
        j       handle_menu_text_entry_generic_specific
        addiu   t2, t2, %lo(@address_bench_unit_select_text)
    
    handle_menu_text_entry_generic_unbench_unit_select_text:
        lui     t2, %hi(@address_unbench_unit_select_text)
        j       handle_menu_text_entry_generic_specific
        addiu   t2, t2, %lo(@address_unbench_unit_select_text)
        
    handle_menu_text_entry_generic_bench_menu_select_text:
        lui     t2, %hi(@address_bench_menu_select_text)
        j       handle_menu_text_entry_generic_specific
        addiu   t2, t2, %lo(@address_bench_menu_select_text)
    
    handle_menu_text_entry_generic_specific:
        la      t1, @world_display_specific_menu_text
        bne     t0, t1, handle_menu_text_entry_generic_end
        move    v0, t2
                
        move    a0, a1
        move    a1, a2
        jal     @world_display_specific_menu_text
        move    a2, t2
    
    handle_menu_text_entry_generic_end:
        lw      ra, 4(sp)
        addiu   sp, sp, 16
        jr      ra
        nop

        
#   HOOK: Formation screen processing (start)
#       Returns:
#           v0 = *0x8018ba1c (word)

@formation_screen_frame_processing_start_hook:
                
        lbu     t0, @address_bench_menu_status
        nop
        beq     t0, zero, formation_screen_frame_processing_start_hook_default
        nop
        
        jal     @formation_screen_unit_menu_processing
        nop
        
        la      ra, 0x80114744
        
    formation_screen_frame_processing_start_hook_default:
        lw      v0, 0x8018ba1c
        jr      ra
        nop
        
        
#   HOOK: Formation screen processing (action check)
#       Returns:
#           v0 = *0x801cd728 (halfword, signed)

@formation_screen_frame_processing_action_check_hook:

        addiu   sp, sp, -24
        sw      s0, 4(sp)
        sw      s1, 8(sp)
        sw      ra, 12(sp)

        lh      s0, 0x801cd728
        li      t2, 1
        addiu   t0, s0, -5
        sltiu   t1, t0, 2
        beq     t1, zero, formation_screen_frame_processing_action_check_hook_end
        addiu   s1, t0, 1
        
        beq     s1, t2, formation_screen_frame_processing_action_check_hook_benchable_check
        nop
        
        jal     @get_first_empty_active_party_slot
        li      a0, 0
        bne     v0, zero, formation_screen_frame_processing_action_check_hook_default
        nop
        
    formation_screen_frame_processing_action_check_hook_benchable_check:
        lbu     t0, @address_selected_formation_unit_index
        nop
        sll     t0, t0, 2
        lw      t0, @address_formation_unit_table (t0)
        nop
        lhu     a0, 0x2c(t0)                #   unitIndex
        jal     @get_party_data_pointer
        nop
        jal     @is_unit_benchable
        move    a0, v0
        bne     v0, zero, formation_screen_frame_processing_action_check_hook_default
        li      t0, 48
        
        #   Invalid choice - Would need to bench unit, but unit not benchable.   Cancel action.
        lui     at, %hi(@address_sound_effect_id)
        j       formation_screen_frame_processing_action_check_hook_end
        sb      t0, %lo(@address_sound_effect_id) (at)
        
    formation_screen_frame_processing_action_check_hook_default:
        #la      t1, @address_formation_show_unit_info_panel
        #lbu     t2, 0(t1)
        sb      s1, @address_bench_menu_status
        #sb      t2, @address_formation_show_unit_info_panel_stored_value
        #sb      zero, 0(t1)
        
        jal     @generate_bench_unit_menu_data
        move    a0, s1
        
    formation_screen_frame_processing_action_check_hook_end:
        move    v0, s0
        
        lw      ra, 12(sp)
        lw      s1, 8(sp)
        lw      s0, 4(sp)
        jr      ra
        addiu   sp, sp, 24


#   ROUTINE: Formation Screen Unbench Inner Menu Processing
@formation_screen_unbench_inner_menu_processing:

        addiu   sp, sp, -16
        sw      s0, 4(sp)
        sw      ra, 8(sp)

        la      a1, @address_unbench_inner_menu_data
        jal     @world_run_menu_thread
        li      a0, 6

        bne     v0, zero, formation_screen_unbench_inner_menu_processing_end
        nop
        
        #   Selected entry / cancel processing
        lh      t3, @address_unbench_inner_menu_selection
        lui     t2, %hi(@address_unbench_inner_menu_status)
        lui     t6, %hi(@address_bench_menu_status)
        lbu     t1, %lo(@address_unbench_inner_menu_status) (t2)
        sb      zero, %lo(@address_unbench_inner_menu_status) (t2)
        
        #   If canceled, return
        bltz    t3, formation_screen_unbench_inner_menu_processing_end
        lui     t4, %hi(@address_sound_effect_id)
        
        #   Otherwise, handle action
        lh      s0, @address_bench_menu_selection
        li      t5, 1
        bne     t3, zero, formation_screen_unbench_inner_menu_processing_handle_dismiss
        move    a1, s0
        
        #   Handling of Unbench action
        lbu     a0, @address_selected_formation_unit_index
        sb      t5, %lo(@address_sound_effect_id) (t4)
        jal     @handle_unit_unbench
        sb      zero, %lo(@address_bench_menu_status) (t6)
        j       formation_screen_unbench_inner_menu_processing_end
        nop
        
        #   Handling of Dismiss action
    formation_screen_unbench_inner_menu_processing_handle_dismiss:
        li      t5, 9
        sb      t5, %lo(@address_sound_effect_id) (t4)
        #sb      zero, %lo(@address_bench_menu_status) (t6)
        jal     @get_bench_slot_pointer
        move    a0, s0
        lbu     t0, 0(v0)
        sb      zero, 0(v0)
        sb      t0, @address_restore_unit_array (s0)
        jal     @generate_bench_unit_menu_data
        li      a0, 2
        
    formation_screen_unbench_inner_menu_processing_end:
        lw      ra, 8(sp)
        lw      s0, 4(sp)
        jr      ra
        addiu   sp, sp, 16

        
#   ROUTINE: OBTAIN PARTY UNIT ITEMS
#       Gives all a party unit's items to the player.
#       For each item, if already over maximum inventory for the item, sells it for price / 4 gil and caps gil at 99999999.
#       Parameters:
#           a0 = (partyUnit) Party unit data pointer

@obtain_party_unit_items:
        addiu   sp, sp, -24
        sw      ra, 4(sp)
        sw      s0, 8(sp)
        sw      s1, 12(sp)
        
        move    s0, a0
        li      s1, 0
        li      t1, 0xff
    obtain_party_unit_items_loop:
        addu    t0, s0, s1
        lbu     a0, 0x0e(t0)
        jal     @obtain_item
        sb      t1, 0x0e(t0)
        addiu   s1, s1, 1
        sltiu   t0, s1, 7
        bne     t0, zero, obtain_party_unit_items_loop
        li      t1, 0xff
        
        lw      s1, 12(sp)
        lw      s0, 8(sp)
        lw      ra, 4(sp)
        addiu   sp, sp, 24
        jr      ra
        nop

        
#   ROUTINE: OBTAIN ITEM
#       Gives an item to the player.  If already over maximum inventory for the item, sells it for price / 4 gil and caps gil at 99999999.
#       Parameters:
#           a0 = (itemID) ID of the item to give to the player

@obtain_item:        
        addiu   sp, sp, -32
        sw      ra, 4(sp)
        sw      s0, 8(sp)

        beq     a0, zero, obtain_item_end
        li      t0, 0xff
        beq     a0, t0, obtain_item_end
        nop
        
        move    s0, a0
        jal     @get_total_equipment_quantity   #   get total equipment quantity (itemID, 0);
        li      a1, 0               #       (Use party data)
        sltiu   t0, v0, 99
        beq     t0, zero, obtain_item_gil
        li      v0, 0
        la      t0, @address_item_quantities_array
        addu    t0, t0, s0          #   quantity = Item quantity for this item
        lbu     t1, 0(t0)           #   quantity
        nop
        addiu   t1, t1, 1           #   quantity + 1
        j       obtain_item_end			
        sb      t1, 0(t0)           #   quantity = quantity + 1
        
    obtain_item_gil:
        sll     t0, s0, 3           #   itemID * 8
        sll     t1, s0, 2           #   itemID * 4
        addu    t0, t0, t1          #   itemID * 12
        lhu     a0, @address_item_data_price (t0)   #   price
        jal     @world_obtain_gil   #   obtain gil (price / 4);
        srl     a0, a0, 2
        
    obtain_item_end:
        lw      s0, 8(sp)
        lw      ra, 4(sp)
        addiu   sp, sp, 32
        jr      ra
        nop
        