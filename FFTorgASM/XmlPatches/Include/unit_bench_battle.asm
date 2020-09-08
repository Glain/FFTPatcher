.label  @get_party_data_pointer, 0x80059af0
.label  @save_unit_to_party, 0x80059bb0
.label  @remove_unit_from_party, 0x80059fe0
.label  @copy_bytes, 0x8005e254
.label  @get_total_equipment_quantity, 0x8005e288

.label  @post_battle_get_free_party_slot_status, 0x801c6000

.label  @address_initial_bench_location, 0x800579c8
.label  @address_item_quantities_array, 0x800596e0
.label  @address_action_status, 0x8018f5fc


#   ROUTINE: Get join status (post-battle)
#       Parameters:
#           a0 = Battle unit index
#       Returns:
#           v0 = 1 if unit can join, 0 otherwise

@post_battle_get_join_status:

        addiu   sp, sp, -16
        sw      ra, 4(sp)
        
        sb      zero, @address_is_bench_join
        
        jal     @post_battle_get_free_party_slot_status
        nop

        bne     v0, zero, post_battle_get_join_status_end
        nop
        
        jal     @battle_get_first_empty_bench_slot
        nop
        
        sltu    v0, zero, v0
        sb      v0, @address_is_bench_join
        
    post_battle_get_join_status_end:
        lw      ra, 4(sp)
        addiu   sp, sp, 16
        jr      ra
        nop


#   ROUTINE: Post-battle join
#       Parameters:
#           a0 = (unit) Unit in-battle data pointer
#           a1 = (isGuest) Whether to save as guest

@post_battle_join: 

        addiu   sp, sp, -16
        sw      s0, 4(sp)
        sw      ra, 8(sp)

        lbu     t0, @address_is_bench_join
        sltiu   t1, a1, 1
        and     t0, t0, t1
        beq     t0, zero, post_battle_join_default
        nop

        jal     @obtain_battle_unit_items
        move    s0, a0
        
        jal     @bench_new_unit
        move    a0, s0
        
        j       post_battle_join_end
        nop
        
    post_battle_join_default:
        jal     @save_unit_to_party
        nop
        
    post_battle_join_end:
        lw      ra, 8(sp)
        lw      s0, 4(sp)
        jr      ra
        addiu   sp, sp, 16
        
        
#   ROUTINE: Bench new unit
#       Parameters:
#           a0 = Battle unit pointer

@bench_new_unit:

        addiu   sp, sp, -272
        sw      ra, 4(sp)

        addiu   a1, sp, 8
        jal     @save_battle_unit_as_party_unit
        li      a2, 0
        
        jal     @battle_get_first_empty_bench_slot
        nop
        
        beq     v0, zero, bench_new_unit_end
        addiu   a0, sp, 8
        
        jal     @battle_bench_unit
        move    a1, v0
       
    bench_new_unit_end:
        lw      ra, 4(sp)
        addiu   sp, sp, 272
        jr      ra
        nop


#   ROUTINE: Save battle unit as party unit
#       Parameters:
#           a0 = Battle unit pointer
#           a1 = Party unit pointer
#           a2 = Party unit index

@save_battle_unit_as_party_unit:

        addiu   sp, sp, -40
        sw      s2, 32(sp)
        sw      ra, 36(sp)
        sw      s1, 28(sp)
        sw      s0, 24(sp)
        move    s2, a0
        move    s1, a2
        j       0x80059c60
        move    v0, a1
        
        
#   ROUTINE: Bench unit to first available slot
#       Parameters:
#           a0 = Party unit index

#@bench_unit_to_first_available_slot:
#
#        addiu   sp, sp, -24
#        sw      s0, 4(sp)
#        sw      ra, 8(sp)
#
#        jal     @battle_get_first_empty_bench_slot
#        move    s0, a0
#       
#        move    a0, s0
#        jal     @handle_unit_bench_simple
#        move    a1, v0
#        
#        lw      ra, 8(sp)
#        lw      s0, 4(sp)
#        jr      ra
#        addiu   sp, sp, 24

        
#   ROUTINE: Handle unit bench (simple)
#       Parameters:
#           a0 = Party unit index
#           a1 = Bench slot index
#
#@handle_unit_bench_simple:
#
#        addiu   sp, sp, -32
#        sw      s0, 4(sp)
#        sw      s1, 8(sp)
#        sw      ra, 12(sp)
#        
#        move    s0, a0                      #   partyUnitIndex
#        
#        jal     @battle_get_bench_slot_pointer
#        move    a0, a1
#                
#        move    a0, s0
#        jal     @get_party_data_pointer
#        move    s1, v0                      #   benchSlot
#        
#        move    a0, v0
#        jal     @battle_bench_unit
#        move    a1, s1
#        
#        jal     @remove_unit_from_party
#        move    a0, s0
#        
#        lw      ra, 12(sp)
#        lw      s1, 8(sp)
#        lw      s0, 4(sp)
#        jr      ra
#        addiu   sp, sp, 32

        

#   ROUTINE: Bench unit (BATTLE.BIN)
#       Parameters:
#           a0 = Party unit data pointer
#           a1 = Bench slot pointer

@battle_bench_unit:

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
        
        #   For monsters, store equipped abilities data into first learned ability bytes, and return!
        lbu     t1, 4(s0)
        addiu   a0, s0, 7
        andi    t0, t1, 0x20    
        beq     t0, zero, battle_bench_unit_past_monster_check
        addiu   a1, s1, 22
        jal     @copy_bytes
        li      a2, 8
        j       battle_bench_unit_end
        nop
        
    battle_bench_unit_past_monster_check:   
        #   Learned abilities, R/S/M, job levels
        li      t0, 0       #   Job index (0 to 19)
        li      a0, 0       #   Current value for R/S/M
        li      a1, 0       #   Current value for Job Levels
    battle_bench_unit_job_loop:
        sll     t2, t0, 1
        addu    t4, s1, t2
        addu    t2, t2, t0
        addu    t3, s0, t2
        
        #   Skip to R/S/M check, if (index == 19)
        li      t5, 19
        beq     t0, t5, battle_bench_unit_job_loop_rsm_check
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
        
    battle_bench_unit_job_loop_rsm_check:
        #   R/S/M - Check if ((index + 1) % 4) == 0, and if not, skip to next section (Job Levels)
        addiu   t6, t0, 1
        andi    t7, t6, 0x03
        bne     t7, zero, battle_bench_unit_job_loop_past_rsm
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
        
    battle_bench_unit_job_loop_past_rsm:   
        #   Job Levels        
        andi    t7, t0, 1
        bne     t7, zero, battle_bench_unit_job_loop_past_job_level_load
        andi    t5, t9, 0x0f
        
        srl     t3, t0, 1
        addu    t3, s0, t3
        lbu     t9, 0x64(t3)        #   Loaded job level value
        nop
        srl     t5, t9, 4
        
    battle_bench_unit_job_loop_past_job_level_load:
        sltiu   t7, t5, 1
        addu    t5, t5, t7          #   If value == 0, set value = 1
        addiu   t5, t5, -1          #   (1 to 8) -> (0 to 7)
        
        sll     a1, a1, 3
        or      a1, a1, t5          #   Add to current value
        
        #   Job levels - Check if ((index + 1) % 8) == 0, and if not, skip to next section
        addiu   t6, t0, 1
        andi    t7, t6, 0x07
        bne     t7, zero, battle_bench_unit_job_loop_past_job_levels
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
        
    battle_bench_unit_job_loop_past_job_levels:
        #   Bottom of loop - check condition
        sltiu   t1, t0, 19
        bne     t1, zero, battle_bench_unit_job_loop
        addiu   t0, t0, 1
        
        #   Write remaining job levels
        sll     t0, a1, 4
        sb      t0, 82(s1)
        srl     t0, t0, 8
        sb      t0, 81(s1)
        
    battle_bench_unit_end:
        #   Return!
        lw      ra, 12(sp)
        lw      s1, 8(sp)
        lw      s0, 4(sp)
        jr      ra
        addiu   sp, sp, 24

        
        
#   ROUTINE: Get Bench Slot Pointer (BATTLE.BIN)
#       Parameters:
#           a0 = Bench slot index
#       Returns:
#           v0 = Bench slot pointer

@battle_get_bench_slot_pointer:

        lui     t1, %hi(@address_initial_bench_location)
        sltiu   t2, a0, 4
        bne     t2, zero, battle_get_bench_slot_pointer_calc
        addiu   t1, t1, %lo(@address_initial_bench_location)
        
        la      t1, @extra_save_data_location
        addiu   a0, a0, -4
        
    battle_get_bench_slot_pointer_calc:  
        sll     t0, a0, 2                   #   benchSlotIndex * 4
        addu    t0, t0, a0                  #   benchSlotIndex * 5
        sll     t0, t0, 2                   #   benchSlotIndex * 20
        addu    t0, t0, a0                  #   benchSlotIndex * 21
        sll     t0, t0, 2                   #   benchSlotIndex * 84
    
        jr      ra
        addu    v0, t1, t0

        
#   ROUTINE: Get first empty bench slot (BATTLE.BIN)
#       Gets the first empty bench slot pointer.
#       Returns:
#           v0 = Bench slot pointer of first unused bench slot, or null (0) if no empty slots exist.

@battle_get_first_empty_bench_slot:

        addiu   sp, sp, -16
        sw      s0, 4(sp)
        sw      ra, 8(sp)
        
        li      s0, 0
        
    battle_get_first_empty_bench_slot_loop:
        jal     @battle_get_bench_slot_pointer
        move    a0, s0
        
        lbu     t0, 0(v0)
        nop
        beq     t0, zero, battle_get_first_empty_bench_slot_end
        addiu   s0, s0, 1
        
        sltiu   t0, s0, 10
        bne     t0, zero, battle_get_first_empty_bench_slot_loop
        li      v0, 0
        
    battle_get_first_empty_bench_slot_end:
        lw      ra, 8(sp)
        lw      s0, 4(sp)
        jr      ra
        addiu   sp, sp, 16


#   ROUTINE: OBTAIN BATTLE UNIT ITEMS
#       Gives all a battle unit's items to the player.
#       For each item, if already over maximum inventory for the item, sells it for price / 4 gil and caps gil at 99999999.
#       Parameters:
#           a0 = (battleUnit) Battle unit data pointer

@obtain_battle_unit_items:
        addiu   sp, sp, -24
        sw      ra, 4(sp)
        sw      s0, 8(sp)
        sw      s1, 12(sp)
        
        move    s0, a0
        li      s1, 0
        li      t1, 0xff
    obtain_battle_unit_items_loop:
        addu    t0, s0, s1
        lbu     a0, 0x1a(t0)
        jal     @battle_obtain_item
        sb      t1, 0x1a(t0)
        addiu   s1, s1, 1
        sltiu   t0, s1, 7
        bne     t0, zero, obtain_battle_unit_items_loop
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

@battle_obtain_item:        
        addiu   sp, sp, -32
        sw      ra, 4(sp)
        sw      s0, 8(sp)

        beq     a0, zero, battle_obtain_item_end
        li      t0, 0xff
        beq     a0, t0, battle_obtain_item_end
        nop
        
        move    s0, a0
        jal     @get_total_equipment_quantity   #   get total equipment quantity (itemID, 1);
        li      a1, 1               #       (Use battle data)
        sltiu   t0, v0, 99		    
        beq     t0, zero, battle_obtain_item_gil	
        li      v0, 0
        la      t0, @address_item_quantities_array  #   Item quantities array
        addu    t0, t0, s0          #   quantity = Item quantity for this item
        lbu     t1, 0(t0)           #   quantity
        nop
        addiu   t1, t1, 1           #   quantity + 1
        j       battle_obtain_item_end			
        sb      t1, 0(t0)           #   quantity = quantity + 1
        
    battle_obtain_item_gil:
        sb      zero, 21(sp)
        move    a2, s0              #   itemID
        la      t0, @address_action_status
        lw      s0, 0(t0)
        addiu   a0, sp, 16          #   Hack: set specific value for routine
        sw      zero, 0(t0)
        jal     0x8018ea98          #   Get price / 4 gil for item (capped at 99999999 total gil)   
        li      a1, 0
        sw      s0, @address_action_status
        
    battle_obtain_item_end:
        lw      s0, 8(sp)
        lw      ra, 4(sp)
        addiu   sp, sp, 32
        jr      ra
        nop
        