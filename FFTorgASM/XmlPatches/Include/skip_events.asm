.label  @address_controller_input, 0x80045944
.label  @address_current_event_id, 0x800577b8
.label  @address_map_id, 0x800577e8
.label  @address_dialog_text_skip_destination, 0x8013276c
.label  @address_event_instruction_draw_destination, 0x801455d8
.label  @address_event_instruction_warp_destination, 0x80145830
.label  @address_event_instruction_skip_destination, 0x80145f24
.label  @address_event_instruction_wait_value_skip_destination, 0x8014a44c
.label  @address_event_instruction_wait_inner_loop_start, 0x8014c874
.label  @address_event_parameter_length_array, 0x8014d170
.label  @address_thread_array, 0x8016986c
.label  @address_current_event_pointer, 0x80173ca4
.label  @address_current_thread_id, 0x80174038

.label  @address_calc_skip_type, 0x8015bb85
.label  @address_is_dialog_text_unskippable, 0x8015bb86
.label  @address_are_effects_unskippable, 0x8015bb87
.label  @address_is_event, 0x8015bb88
.label  @address_is_skipping_effect_targets, 0x8015bb89
.label  @address_is_skipping_effect, 0x8015bb8a
.label  @address_run_next_wait, 0x8015bb8b
.label  @address_pathing_direction_map, 0x8015bb8c
.label  @address_event_last_camera_instruction_pointer, 0x8015bb90
.label  @address_event_last_focus_instruction_pointer, 0x8015bb94
.label  @address_event_choice_message_id, 0x8015bb98
.label  @address_event_choice_message_id_display_message, 0x8015bb9a
.label  @address_event_skip_type, 0x8015bb9c
.label  @address_event_old_skip_type, 0x8015bb9d
.label  @address_is_mid_battle, 0x8015bb9e
.label  @address_is_mid_battle_event, 0x8015bb9f
.label  @address_can_skip_instruction_array, 0x8015bba0
.label  @address_can_total_skip_instruction_array, 0x8015bbc0

.label  @get_unit_misc_data_pointer, 0x8007a6e4
.label  @color_field, 0x80093170
.label  @color_background_tiles_set_1, 0x8009349c
.label  @color_background_tiles_set_2, 0x80093504
.label  @get_unit_misc_id_from_entd_unit_id, 0x80133158
.label  @set_event_speed, 0x8013da00
.label  @camera_event_instruction, 0x80146110
.label  @warp_unit, 0x80147e60
.label  @color_units, 0x801495e0
.label  @switch_to_next_thread, 0x8014ca80
.label  @calculate_walkto_pathing, 0x801780ec

.eqv    %start_button_flag, 0x0800
.eqv    %wait_frame_cap_plus_one, 31
.eqv    %wait_frame_cap, 30

.org    0x8015bc00

#   ROUTINE: Check event button input
#       Returns:
#           a1 = Button input value (*0x80045944) (word)
#       Preserve: v0
@check_event_button_input:

        addiu   sp, sp, -24
        sw      ra, 4(sp)
        sw      s0, 8(sp)
        sw      v0, 12(sp)

        lw      s0, @address_controller_input
        nop
        
        #   If start isn't pressed, skip to the end.
        andi    t0, s0, %start_button_flag
        beq     t0, zero, check_event_button_input_end
        nop
        
        #   If we're already skipping the event, skip to the end.
        lbu     t0, @address_event_skip_type
        nop
        bne     t0, zero, check_event_button_input_end
        nop
        
        #   Only set event skip type if an event is actually running!
        lbu     t0, @address_is_event
        nop
        beq     t0, zero, check_event_button_input_end
        nop
        
        #   Force event back to normal speed if it was slowed down
        #jal     @set_event_speed
        #li      a0, 1
        
        #   Find event skip type
        jal     @get_event_skip_type
        nop
        sb      v0, @address_event_skip_type
        
    check_event_button_input_end:
        move    a1, s0
        
        lw      ra, 4(sp)
        lw      s0, 8(sp)
        lw      v0, 12(sp)
        addiu   sp, sp, 24
        jr      ra
        nop 
       
#   HOOK: Event start
#       Returns:
#           v0 = *0x80165ff6 (halfword, unsigned)
@event_start_hook:
        addiu   sp, sp, -16
        sw      ra, 4(sp)

        sb      zero, @address_event_skip_type
        sb      zero, @address_event_old_skip_type
        
        jal     @get_event_skip_type
        nop
        
        sb      zero, @address_run_next_wait
        li      t0, 1
        sb      t0, @address_is_event
        lhu     v0, 0x80165ff6
        
        lw      ra, 4(sp)
        addiu   sp, sp, 16
        jr      ra
        nop

#   ROUTINE: Get event skip type
#   Determines which "skip type" to use for this event (1 = Partial, 2 = Total)
@get_event_skip_type:

        li      t6, -2                                                  #   Message ID of last DisplayMessage command
        sh      t6, @address_event_choice_message_id                    #   ChoiceMessageID = -2 (none); i.e. So far, there is no message where the player makes a choice
        li      t7, -2
        sh      t7, @address_event_choice_message_id_display_message    #   Last DisplayMessage ID before choice = -2 (none)
        sw      zero, @address_event_last_camera_instruction_pointer
        sw      zero, @address_event_last_focus_instruction_pointer

        #   If we already skipped an event with type = 1, we can't totally skip any cutscenes until the next event block.  (i.e., We're in a mid-battle cutscene)
        lbu     t0, @address_is_mid_battle
        li      v0, 1
        sb      t0, @address_is_mid_battle_event
        beq     t0, v0, get_event_skip_type_end
        nop
        
        lw      t0, @address_current_event_pointer
        la      t3, @address_event_parameter_length_array
        li      v0, 2
        
    get_event_skip_type_loop:
        lbu     t1, 0(t0)       #   Event instruction ID
        
        li      t2, 0xdb        #   EventEnd
        beq     t1, t2, get_event_skip_type_end
        li      t2, 0xe3        #   EventEnd2
        beq     t1, t2, get_event_skip_type_end
        
        li      t2, 0x78        #   DisplayConditions
        beq     t1, t2, get_event_skip_type_loop_set
        li      t2, 0xb1        #   AddVar
        beq     t1, t2, get_event_skip_type_check_addvar
        li      t2, 0x10        #   DisplayMessage
        beq     t1, t2, get_event_skip_type_check_display_message
        li      t2, 0x51        #   ChangeDialog
        beq     t1, t2, get_event_skip_type_check_change_dialog
        li      t2, 0x19
        beq     t1, t2, get_event_skip_type_check_camera
        li      t2, 0x1f
        beq     t1, t2, get_event_skip_type_check_focus
        nop
        j       get_event_skip_type_loop_continue
        nop
        
    get_event_skip_type_check_addvar:
        lbu     t4, 3(t0)       #   AddVar addend
        li      t5, 0x18        #   Variable 0x18 = Selected Option in Dialog
        bne     t4, t5, get_event_skip_type_loop_continue
        nop
        sh      t6, @address_event_choice_message_id
        sh      t7, @address_event_choice_message_id_display_message
        j       get_event_skip_type_loop_set
        nop
        
    get_event_skip_type_check_display_message:
        lbu     t5, 5(t0)       #   Unit ID
        nop
        sltiu   t5, t5, 4       #   Check if Unit is Ramza; if not, skip
        beq     t5, zero, get_event_skip_type_loop_continue
        nop
        
        lb      t4, 4(t0)
        lbu     t6, 3(t0)       
        sll     t4, t4, 8
        or      t6, t4, t6      #   Message ID
        j       get_event_skip_type_loop_continue
        move    t7, t6
        
    get_event_skip_type_check_change_dialog:
        lb      t4, 3(t0)
        lbu     t5, 2(t0)       
        sll     t4, t4, 8
        or      t5, t4, t5      #   Message ID
        li      t8, -1          #   Skip if Message ID == -1
        beq     t5, t8, get_event_skip_type_loop_continue
        nop
        move    t6, t5
        j       get_event_skip_type_loop_continue
        nop
        
    get_event_skip_type_check_camera:
        sw      t0, @address_event_last_camera_instruction_pointer
        j       get_event_skip_type_loop_continue
        nop
        
    get_event_skip_type_check_focus:
        sw      t0, @address_event_last_focus_instruction_pointer
        j       get_event_skip_type_loop_continue
        nop
    
    get_event_skip_type_loop_set:
        #j       get_event_skip_type_end
        li      v0, 1
       
    get_event_skip_type_loop_continue:
        #   Go to the next instruction
        addu    t2, t3, t1      #   Address inside the instruction parameter length array for this event instruction
        lbu     t2, 0(t2)       #   Load the instruction parameter length
        addiu   t0, t0, 1       #   Add 1 to the current instruction pointer (Now points to the start of the parameters for this instruction)
        addu    t0, t0, t2      #   Add parameter length to the current instruction pointer (Now points to the next instruction)
        
        j       get_event_skip_type_loop
        nop
        
    get_event_skip_type_end:
        jr      ra
        nop
        
#   HOOK: Check Event Skip
#   Main event instruction hook. Skips certain instructions and transforms others.
#       Parameters:
#           s1 = Pointer to parameters for this event instruction
#           s2 = Event Instruction Parameter Byte Index 0
#           s3 = Event Instruction Parameter Halfword Index 0 (Index 0 and Index 1, Combined)
#           s4 = Event Instruction ID
#           s5 = Event Instruction Parameter Byte Index 1
#           s6 = Event Instruction Parameter Byte Index 2
#           s7 = Event Instruction Parameter Byte Index 3
#       Returns:
#           v0 = 0xc0
@check_event_skip:   

        addiu   sp, sp, -40
        sw      s0, 4(sp)
        move    s0, ra
        
        #   If we're running the next event instruction, dialog text is no longer unskippable.
        #sb      zero, @address_is_dialog_text_unskippable
        
        li      t0, 0xdb        #   EventEnd
        beq     s4, t0, check_event_skip_reset_on_event_end
        li      t0, 0xe3        #   EventEnd2
        beq     s4, t0, check_event_skip_reset_on_event_end
        nop
        
        #   If not EventEnd or EventEnd2, we must be running an event.
        li      t0, 1
        sb      t0, @address_is_event
        
        #   If this is a compound event (one event rolling directly into another, e.g. post-Limberry cutscene), calculate the skip type.
        lbu     t0, @address_event_old_skip_type
        nop
        bne     t0, zero, check_event_skip_calc_skip_type
        nop
        #sb      t0, @address_event_skip_type
        
        #   If we're scheduled to calculate the skip type, do so.
        lbu     t0, @address_calc_skip_type
        nop
        bne     t0, zero, check_event_skip_calc_skip_type
        nop
        
        #   Otherwise, skip the calculation.
        j       check_event_skip_pre_checks
        nop
        
    check_event_skip_calc_skip_type:    
        jal     @get_event_skip_type
        nop
        sb      v0, @address_event_skip_type
        
        sb      zero, @address_event_old_skip_type
        sb      zero, @address_calc_skip_type
     
    check_event_skip_pre_checks:
        li      t0, 0x44        #   Draw
        beq     s4, t0, check_event_skip_set_run_next_wait
        li      t0, 0x3d        #   RemoveUnit
        beq     s4, t0, check_event_skip_set_run_next_wait
        li      t0, 0x45        #   AddUnit
        beq     s4, t0, check_event_skip_set_run_next_wait
        li      t0, 0x96        #   AppendMapState
        beq     s4, t0, check_event_skip_check_append_map_state
        nop
        j       check_event_skip_check_display_conditions
        nop
        
    check_event_skip_reset_on_event_end:
        lbu     t0, @address_event_skip_type
        sb      zero, @address_is_event
        sb      t0, @address_event_old_skip_type
        sb      zero, @address_event_skip_type
        sb      zero, @address_are_effects_unskippable
        j       check_event_skip_check_display_conditions
        nop
        
    check_event_skip_set_run_next_wait:
        #   Run next wait, but only if not total skipping (Event skip type != 2)
        lbu     t1, @address_event_skip_type
        li      t0, 1
        sltiu   t1, t1, 2
        and     t0, t0, t1
        sb      t0, @address_run_next_wait
        j       check_event_skip_check_display_conditions
        nop
        
    check_event_skip_check_append_map_state:
        #   If we're running AppendMapState, effects are now unskippable (until event end).
        lbu     t0, @address_event_skip_type
        nop
        bne     t0, zero, check_event_skip_check_display_conditions
        li      t1, 1
        
        sb      t1, @address_are_effects_unskippable
        
        #j       check_event_skip_check_display_conditions
        #nop
        
    check_event_skip_check_display_conditions:
        #   Check for DisplayConditions instruction to see if the "in battle" indicator can be cleared
        li      t0, 0x78
        bne     s4, t0, check_event_skip_type
        li      t0, 2
        beq     s2, t0, check_event_skip_set_is_mid_battle
        li      t0, 0
        bne     s2, zero, check_event_skip_type
        li      t0, 1  
    
    check_event_skip_set_is_mid_battle:
        sb      t0, @address_is_mid_battle 
        
    check_event_skip_type:    
        lbu     v0, @address_event_skip_type
        nop
        beq     v0, zero, check_event_skip_end
        li      t0, 2
        
        la      v1, @address_can_skip_instruction_array
        bne     v0, t0, check_event_skip_check_unskippable_effects
        nop
        
        la      v1, @address_can_total_skip_instruction_array
    
    check_event_skip_check_unskippable_effects:
        lbu     t0, @address_are_effects_unskippable
        nop
        beq     t0, zero, check_event_skip_check_specific
        nop
        
        li      t0, 0x18        #   0x18 = LoadEffect (Effect)
        beq     s4, t0, check_event_skip_end
        li      t0, 0x8a        #   0x8a = EffectStart
        beq     s4, t0, check_event_skip_end
        li      t0, 0x8b        #   0x8b = PlayEffect (EffectEnd)
        beq     s4, t0, check_event_skip_end
        nop
    
    check_event_skip_check_specific:
        #   Check specific instructions
        li      t0, 0x10        #   0x10 = DisplayMessage
        beq     s4, t0, check_event_skip_process_display_message
        li      t0, 0x19        #   0x19 = Camera
        beq     s4, t0, check_event_skip_process_camera
        li      t0, 0x1f        #   0x1f = Focus
        beq     s4, t0, check_event_skip_process_focus
        li      t0, 0x28        #   0x28 = WalkTo
        beq     s4, t0, check_event_skip_process_walk_to
        li      t0, 0x3b        #   0x3b = SpriteMove
        beq     s4, t0, check_event_skip_process_sprite_move
        li      t0, 0x3e        #   0x3e = ColorScreen
        beq     s4, t0, check_event_skip_process_color_screen
        li      t0, 0x51        #   0x51 = ChangeDialog
        beq     s4, t0, check_event_skip_process_change_dialog
        li      t0, 0x6e        #   0x63 = SpriteMoveBeta
        beq     s4, t0, check_event_skip_process_sprite_move_beta
        li      t0, 0x8b        #   0x8b = PlayEffect (EffectEnd)
        beq     s4, t0, check_event_skip_process_play_effect
        li      t0, 0xf1        #   0xf1 = Wait
        beq     s4, t0, check_event_skip_process_wait
        nop
       
        j       check_event_skip_generic
        nop
        
    check_event_skip_process_display_message:
        #   Check if this is a player choice
        lh      t0, @address_event_choice_message_id_display_message
        lb      t2, 3(s1)
        lbu     t1, 2(s1)       #   Message ID
        sll     t2, t2, 8
        or      t1, t2, t1
        beq     t0, t1, check_event_skip_process_display_message_choice
        nop
    
        #   If a normal message (not a player choice dialog), Convert DisplayMessage command into Draw command
        lbu     s3, 4(s1)       #   Set Draw parameter = UnitID
        la      s0, @address_event_instruction_draw_destination
        j       check_event_skip_generic
        nop
        
    check_event_skip_process_display_message_choice:
        #   If this is a player choice, replace the Message ID with the choice Message ID
        lh      t0, @address_event_choice_message_id
        nop
        sb      t0, 2(s1)
        andi    s6, t0, 0xff
        srl     t0, t0, 8
        sb      t0, 3(s1)
        andi    s7, t0, 0xff
        j       check_event_skip_block
        nop
    
    check_event_skip_process_camera:    
        #   Skip Camera commands if in the middle of battle
        lbu     t0, @address_is_mid_battle_event
        nop
        bne     t0, zero, check_event_skip_skip
        nop
        
        #   Skip if this isn't the last Camera instruction
        lw      t0, @address_event_last_camera_instruction_pointer
        addiu   t1, s1, -1
        bne     t0, t1, check_event_skip_skip
        nop
        
        #   Speed up Camera (0x19): Parameter byte 14 (Time) = 1 frame
        li      t0, 1
        sb      t0, 14(s1)      #   Low byte = 1
        j       check_event_skip_generic
        sb      zero, 15(s1)    #   High byte = 0
    
    check_event_skip_process_focus:
        #   Skip Focus commands if in the middle of battle
        lbu     t0, @address_is_mid_battle_event
        nop
        bne     t0, zero, check_event_skip_skip
        nop
        
        #   Skip if this isn't the last Focus instruction
        lw      t0, @address_event_last_focus_instruction_pointer
        addiu   t1, s1, -1
        bne     t0, t1, check_event_skip_skip
        nop
        
        j       check_event_skip_generic
        nop
    
    check_event_skip_process_walk_to:
        #   Speed up WalkTo (0x28): Parameter byte 6 (Speed) = 127 (max)
        #li      t0, 127        
        #j       check_event_skip_generic
        #sb      t0, 6(s1)       #   Speed = 127 (max)
        
        #   Convert WalkTo into WarpUnit
        sw      v1, 8(sp)
        
        #   Get unit misc data pointer
        jal     @get_unit_misc_id_from_entd_unit_id
        move    a0, s3          #   ENTD Unit ID
        jal     @get_unit_misc_data_pointer
        move    a0, v0
        
        #   If the unit misc ID is invalid, pick zero as the facing direction and skip to the set section
        lw      v1, 8(sp)
        beq     v0, zero, check_event_skip_process_walk_to_set_facing_direction
        li      t0, 0
        
        #   Calculate the WalkTo pathing
        lbu     a0, 7(s1)       #   ElevationFlag
        lbu     t1, 4(s1)       #   Parameter HigherElevation
        sll     a0, a0, 8       #   ElevationFlag * 256
        li      t2, 256
        subu    a0, t2, a0      #   Parameter 1: ElevationAddend = (256 - (ElevationFlag * 256))
        li      a1, 3           #   Parameter 2: 3
        lbu     t0, 0x7e(v0)    #   unitMiscData.HigherElevation
        lbu     a2, 0x7c(v0)    #   Parameter 3: unitMiscData.X
        lbu     a3, 0x7d(v0)    #   Parameter 4: unitMiscData.Y      
        sw      t0, 16(sp)      #   Parameter 5: unitMiscData.HigherElevation
        sw      s6, 20(sp)      #   Parameter 6: Parameter X Coordinate  
        sw      s7, 24(sp)      #   Parameter 7: Parameter Y Coordinate
        jal     @calculate_walkto_pathing
        sw      t1, 28(sp)      #   Parameter 8: Parameter HigherElevation
        
        lw      v1, 8(sp)
        
        #   If the WalkTo is invalid and the unit can't get to the tile, pick zero as the facing direction
        beq     v0, zero, check_event_skip_process_walk_to_set_facing_direction
        li      t0, 0
        
        #   Get number of tiles being moved in the WalkTo
        lbu     t0, 0(v0)
        nop
        
        #   If none, set facing direction to 0
        beq     t0, zero, check_event_skip_process_walk_to_set_facing_direction
        addu    t1, v0, t0
        
        #   Get final tile movement
        #       0x00 = Move east
        #       0x40 = Move west
        #       0x80 = Move south
        #       0xc0 = Move north
        lbu     t0, 0(t1)
        nop
        
        #   Find facing direction of final movement and skip to set section
        #       0x00 = South
        #       0x01 = West
        #       0x02 = North
        #       0x03 = East
        la      t1, @address_pathing_direction_map
        
        srl     t0, t0, 6
        addu    t2, t1, t0
        lbu     t0, 0(t2)
        j       check_event_skip_process_walk_to_set_facing_direction
        nop
        
    check_event_skip_process_walk_to_set_facing_direction:
        la      s0, @address_event_instruction_warp_destination
        j       check_event_skip_generic
        sb      t0, 5(s1)       #  Set facing direction
    
    check_event_skip_process_sprite_move:     
        #   Speed up SpriteMove (0x3b): Parameter bytes 10 and 11 (Time) = 1 frame
        li      t0, 1
        sb      t0, 10(s1)      #   Low byte = 1
        j       check_event_skip_generic
        sb      zero, 11(s1)    #   High byte = 0
    
    check_event_skip_process_color_screen:
        #   Speed up ColorScreen (0x3e): Parameter bytes 7 and 8 (Time) = 1 frame
        li      t0, 1
        sb      t0, 7(s1)       #   Low byte = 1
        j       check_event_skip_generic
        sb      zero, 8(s1)     #   High byte = 0
    
    check_event_skip_process_change_dialog:
        #   Run command as normal if this is a player choice
        lh      t0, @address_event_choice_message_id
        lbu     t1, 1(s1)       #   Message ID
        nop
        beq     t0, t1, check_event_skip_block
        li      t2, 0xff
        
        #   Otherwise, skip this command if it doesn't close the dialog box
        bne     t1, t2, check_event_skip_skip
        nop
        
        j       check_event_skip_generic
        nop
        
    check_event_skip_process_sprite_move_beta:
        #   Speed up SpriteMoveBeta (0x6e): Parameter bytes 10 and 11 (Speed) = 65535 (max)
        li      t0, 0xff
        sb      t0, 10(s1)      #   Low byte = 0xff
        j       check_event_skip_generic
        sb      t0, 11(s1)      #   High byte = 0xff
        
    check_event_skip_process_play_effect:
        sw      zero, 0x801b63e8
        j       check_event_skip_generic
        nop
    
    check_event_skip_process_wait:
        lbu     t0, @address_run_next_wait
        nop
        bne     t0, zero, check_event_skip_run_next_wait
        nop
        #sw      v1, 8(sp)
        #jal     @switch_to_next_thread
        #nop
        #lw      v1, 8(sp)
        j       check_event_skip_generic
        nop
        
    check_event_skip_run_next_wait:
        sb      zero, @address_run_next_wait
        sltiu   t0, s3, %wait_frame_cap_plus_one
        
        beq     t0, zero, check_event_skip_run_next_wait_cap_frames
        nop
        j       check_event_skip_block
        nop
        
    check_event_skip_run_next_wait_cap_frames:
        li      s3, %wait_frame_cap                 #   Cap frames for wait
        j       check_event_skip_block
        nop
        
    check_event_skip_generic:
        move    t0, v1
    
        #   Byte to load: ID div 8
        srl     t1, s4, 3
        addu    t0, t0, t1
        lbu     t0, 0(t0)
        
        #   Flag to check: ID mod 8
        andi    t1, s4, 0x07
        li      t2, 0x80
        srlv    t1, t2, t1
        
        and     t0, t0, t1
        beq     t0, zero, check_event_skip_block
        nop
    
    check_event_skip_skip:
        la      s0, @address_event_instruction_skip_destination
    
    check_event_skip_block:
        lbu     t0, @address_event_skip_type
        li      t1, 1
        bne     t0, t1, check_event_skip_end
        nop
        jal     @switch_to_next_thread
        nop
    
    check_event_skip_end:
        li      v0, 0xc0
        
        move    ra, s0
        lw      s0, 4(sp)
        addiu   sp, sp, 40
        jr      ra
        nop

#   HOOK: Advance dialog text
#       Parameters:
#           *(sp + 0x0c) = (New button input)
#       Returns:
#           t1 = 0x200
#       Preserve: v0, t0
@advance_dialog_text_hook:

        lw      t2, 0x0c(sp)
        nop
        andi    t2, t2, %start_button_flag
        beq     t2, zero, advance_dialog_text_hook_end
        nop
        
        #   To skip the rest of the dialog text, set the current character to the FFT text sentinel value 0xFE.
        li      s0, 0xfe    
        
    advance_dialog_text_hook_end:
        jr      ra
        li      t1, 0x200

#   HOOK: Camera event instruction
#       Skips to the end of the event instruction if the event was skipped.
#       Parameters:
#           v0 = Initial continueLoop value (1 = true, 0 = false)
#       Returns:
#           v0 = continueLoop value (1 = true, 0 = false)
@camera_hook:

        lbu     t0, @address_event_skip_type
        nop
        
        beq     t0, zero, camera_hook_end
        nop
        
        li      v0, 0
        
    camera_hook_end:
        jr      ra
        nop

#   HOOK: Camera fusion event instruction
#       Skips to the end of the event instruction if the event was skipped.
#       Returns:
#           v0 = 1
#           v1 = performSkip (1 to skip, 0 if not?); default = *($fp + 0x028c) (word)
@camera_fusion_hook:
        addiu   sp, sp, -16
        sw      ra, 4(sp)

        jal     @check_event_button_input
        nop
        
        lbu     t0, @address_event_skip_type
        nop
        
        beq     t0, zero, camera_fusion_hook_default
        nop
        
        lbu     t0, 24(sp)                          #   Load last instruction in CameraFusion
        li      t1, 0x19                            #   If not Camera, just break out of the CameraFusion
        bne     t0, t1, camera_fusion_hook_end
        li      v1, 1
        
        #   If the final instruction in the CameraFusion is a Camera instuction, run it directly on the current thread.
        lbu     t0, @address_current_thread_id      #   ThreadID
        lw      t1, 28(sp)                          #   Address of final Camera instruction
        sll     t0, t0, 10                          #   ThreadID * sizeof(thread)
        addiu   t1, t1, 1                           #   Address of parameters for final Camera instruction
        sw      t1, @address_thread_array (t0)      #   Thread function parameter = (Address of parameters for final Camera instruction)
        jal     @camera_event_instruction           #   Call Camera event instruction function, which will stop the thread when it finishes
        nop
        
    camera_fusion_hook_default:
        #   Copied from calling routine...
        lw      v1, 0x028c(fp)
        
    camera_fusion_hook_end:
        li      v0, 1
        
        lw      ra, 4(sp)
        addiu   sp, sp, 16
        jr      ra
        nop

#   HOOK: Process single unit movement
#       Parameters:
#           s0 = Unit misc data pointer
#       Returns:
#           v0 = *($s0 + 0x7f) (byte) in the default case, or 0 if the movement is to be skipped    
@process_single_unit_movement_hook:
    
        addiu   sp, sp, -32
        sw      ra, 4(sp)
    
        #   Don't interrupt if the event is not being skipped, and we're not pressing the start button.
        lbu     t0, @address_event_skip_type
        lw      t1, @address_controller_input
        sltu    t0, zero, t0
        andi    t1, t1, %start_button_flag
        sltu    t1, zero, t1
        or      t0, t0, t1
        
        beq     t0, zero, process_single_unit_movement_hook_default
        nop
        
        #   Find relevant values
        la      t2, @address_pathing_direction_map
        lw      t0, 0x98(s0)        #   Number of tiles moved so far
        lbu     t3, 0x7f(s0)
        lbu     t1, 0x9c(s0)        #   (Max Tile Num) = (Number of total movement tiles)
        sltu    t3, zero, t3        #   subtractValue = (0 if miscUnit.0x7f == 0, 1 otherwise)      
        subu    t0, t0, t3          #   (Current Tile Num) = (Number of tiles moved so far) - subtractValue
        lbu     a1, 0x7c(s0)        #   X Coordinate
        lbu     a2, 0x7d(s0)        #   Y Coordinate 
        
        #   If no movement to skip, don't interrupt it
        beq     t1, zero, process_single_unit_movement_hook_default
        #nop
        
        #   If teleporting (Total movement tiles = 0xfe or 0xff), don't interrupt it
        andi    t4, t1, 0xfe
        li      t5, 0xfe
        beq     t4, t5, process_single_unit_movement_hook_default
        
        #   If the movement is already complete, don't try to interrupt it
        sltu    t4, t0, t1
        beq     t4, zero, process_single_unit_movement_hook_default
        nop
        
        #   If skipping the movement, warp the unit to the destination square
    process_single_unit_movement_hook_coords_loop:
        addu    t3, s0, t0
        lbu     t3, 0x9d(t3)        #   Get movement value (0x00 = East, 0x40 = West, 0x80 = South, 0xc0 = North)
        addiu   t0, t0, 1           #   (Current Tile Num) = (Current Tile Num) + 1
        srl     t4, t3, 6           #   (0 = East, 1 = West, 2 = South, 3 = North)
        addu    t5, t2, t4
        lbu     t5, 0(t5)           #   (WarpUnit facing: 0 = South, 1 = West, 2 = North, 3 = East)
        nop
        andi    t6, t5, 0x01        #   Axis (0 = Affects Y, 1 = Affects X)
        srl     t7, t5, 1
        andi    t7, t7, 0x01        #   Direction (0 = Negative, 1 = Positive)
        sll     t6, t6, 31
        sra     t6, t6, 31          #   Axis bitmask (All zeroes = Affects Y, All ones = Affects X)
        sltiu   t8, t7, 1
        subu    t7, t7, t8          #   Direction (-1 = Negative, 1 = Positive)
        and     t8, t7, t6          #   X Offset
        not     t9, t6
        and     t9, t7, t9          #   Y Offset
        
        #   Multiply offsets by (Number of tiles to move) if (Number of tiles to move) != 1 and offsets != 0
        andi    t6, t3, 0x03
        addiu   t6, t6, 1           #   Number of tiles to move
        li      t7, 1
        beq     t6, t7, process_single_unit_movement_hook_coords_loop_past_multiply
        nop
        beq     t8, zero, process_single_unit_movement_hook_coords_loop_past_x_multiply
        nop
        mult    t8, t6
        mflo    t8
    process_single_unit_movement_hook_coords_loop_past_x_multiply:
        beq     t9, zero, process_single_unit_movement_hook_coords_loop_past_multiply
        nop
        mult    t9, t6
        mflo    t9
     
    process_single_unit_movement_hook_coords_loop_past_multiply:     
        addu    a1, a1, t8          #   X Coordinate = X Coordinate + X Offset
        sltu    t4, t0, t1
        bne     t4, zero, process_single_unit_movement_hook_coords_loop
        addu    a2, a2, t9          #   Y Coordinate = Y Coordinate + Y Offset
       
    process_single_unit_movement_hook_coords_post_loop:
        sb      zero, 0x9c(s0)      #   The normal movement is being interrupted, so set the number of movement tiles = 0
        sb      zero, 0x7f(s0)      #   Set miscUnit.0x7f = 0 to reset movement
        
        lw      t0, 0x134(s0)       #   In-battle unit data pointer
        addiu   a0, sp, 16          #   Parameter struct pointer
        lbu     t0, 0x161(t0)       #   ENTD Unit ID
        srl     t1, t3, 5
        andi    t1, t1, 1           #   Higher Elevation value
        
        sb      t0, 0(a0)           #   Parameter byte 0x00: ENTD Unit ID
        sb      zero, 1(a0)         #   Parameter byte 0x01: 0
        sb      a1, 2(a0)           #   Parameter byte 0x02: X Coordinate
        sb      a2, 3(a0)           #   Parameter byte 0x03: Y Coordinate
        sb      t1, 4(a0)           #   Parameter byte 0x04: Higher Elevation
        jal     @warp_unit
        sb      t5, 5(a0)           #   Parameter byte 0x05: Facing Direction
        
        j       process_single_unit_movement_hook_end
        li      v0, 0

    process_single_unit_movement_hook_default:
        lbu     v0, 0x7f(s0)

    process_single_unit_movement_hook_end:
        lw      ra, 4(sp)
        addiu   sp, sp, 32
        jr      ra
        nop

#   HOOK: SpriteMove
#       Parameters:
#           s5 = Current frame
#           fp = Number of frames total
#       Returns:
#           v0 = continueLoop value (1 = true, 0 = false) (Default: (s5 < fp))
@sprite_move_hook:
    
        lbu     t0, @address_event_skip_type
        nop
        beq     t0, zero, _sprite_move_hook_default
        nop
        
        j       _sprite_move_hook_end
        li      v0, 0               #   If skipping, break out of the loop
        
    _sprite_move_hook_default:
        slt     v0, s5, fp
        
    _sprite_move_hook_end:
        jr      ra
        nop

        
#   HOOK: Wait
#   Interrupts wait if the event is being skipped and the frame cap has been reached, or the event is being total skipped.
#       Parameters:
#           v0 = continueLoop value (1 = true, 0 = false)
#           s0 = Frame count
#           s1 = Total frames for Wait
@wait_hook:
        #   Extra condition to continue loop: (Event not being skipped) or (Frame cap not reached yet)
        lbu     t0, @address_event_skip_type
        sltiu   t1, s0, %wait_frame_cap_plus_one
        sltiu   t0, t0, 1
        or      t0, t0, t1
        and     v0, v0, t0

    wait_hook_default:
        beq     v0, zero, wait_hook_end
        nop
        la      ra, @address_event_instruction_wait_inner_loop_start

    wait_hook_end:
        jr      ra
        nop

#   HOOK: Determine dialog text
#   Find if dialog text is unskippable.
#       Parameters:
#           v0 = FFT text pointer
#       Requires:
#           Set s4 = v0
#           Preserve: a0
@determine_dialog_text_hook:
        move    s4, v0

        #   Loop through string looking for 0xFB to determine if text is unskippable
        li      t0, 0xfb
        li      t1, 0xfe
        move    t2, s4
        
    determine_dialog_text_hook_loop_start:   
        lbu     t3, 0(t2)
        addiu   t2, t2, 1
        beq     t3, t0, determine_dialog_text_hook_set_unskippable
        and     t3, t3, t1
        bne     t3, t1, determine_dialog_text_hook_loop_start
        nop
        
        sb      zero, @address_is_dialog_text_unskippable
        j       determine_dialog_text_hook_end
        nop
        
    determine_dialog_text_hook_set_unskippable:
        li      t0, 1
        sb      t0, @address_is_dialog_text_unskippable
    
    determine_dialog_text_hook_end:
        jr      ra
        nop
        
#   HOOK: Skip dialog text
#   Interrupts dialog text if the event is being skipped.
#       Parameters:
#           s0 = Current character byte?
#       Preserve: v0
@skip_dialog_text_hook:

        lbu     t0, @address_is_dialog_text_unskippable
        nop
        bne     t0, zero, skip_dialog_text_hook_default
        nop
        
        lw      t0, @address_controller_input
        lh      t2, @address_event_choice_message_id
        lbu     t3, @address_event_skip_type
        li      t4, -2
        xor     t2, t2, t4                  #   Non-zero if this event has a dialog choice, 0 otherwise
        sltiu   t2, t2, 1                   #   0 if this event has a dialog choice, 1 otherwise
        sltu    t3, zero, t3                #   1 if event is being skipped, 0 otherwise
        and     t2, t2, t3                  #   1 if event is being skipped and doesn't have a dialog choice, 0 otherwise
        la      t1, @address_dialog_text_skip_destination
        andi    t5, t0, %start_button_flag
        or      t0, t5, t2
        beq     t0, zero, skip_dialog_text_hook_default
        nop

        j       skip_dialog_text_hook_end
        move    ra, t1
        
     skip_dialog_text_hook_default:
        li      t0, 3
        bne     v0, t0, skip_dialog_text_hook_end
        nop
        
        move    ra, t1

    skip_dialog_text_hook_end:
        jr      ra
        nop
        
#   HOOK: Show map title
#       Parameters:
#           v0 = continueLoop value (1 = true, 0 = false) (default)
#           t0 = Low order byte of loop start address
@show_map_title_hook:

        lbu     t1, @address_event_skip_type
        nop
        bne     t1, zero, show_map_title_hook_changed
        nop
        
        lw      t1, @address_controller_input
        nop
        andi    t1, t1, %start_button_flag
        beq     t1, zero, show_map_title_hook_default
        nop
        
    show_map_title_hook_changed:
        j       show_map_title_hook_end
        li      v0, 0
        
    show_map_title_hook_default:
        beq     v0, zero, show_map_title_hook_end
        nop
    
        lui     ra, 0x801c
        or      ra, ra, t0
        
    show_map_title_hook_end:
        jr      ra
        nop
 
#   HOOK: Effect
#       Parameters:
#           v0 = continueEffect value (1 = true, 0 = false) (default = *0x801bbf90)
@effect_hook:

        addiu   sp, sp, -32
        sw      ra, 4(sp)
        
        #   Don't skip if the Start button isn't pressed, or if effects are currently unskippable
        lw      t0, @address_controller_input
        lbu     t1, @address_are_effects_unskippable
        andi    t0, t0, %start_button_flag
        sltu    t0, zero, t0
        xori    t1, t1, 1
        and     t0, t0, t1
        beq     t0, zero, effect_hook_default
        nop

    effect_hook_changed:
        jal     0x801a1c90          #   Reset effect variables (0x801bbf90, 0x801b63e8, etc.)
        nop
        
        li      t0, 1
        sb      t0, @address_is_skipping_effect
        sb      t0, @address_is_skipping_effect_targets
        
        j       effect_hook_end
        li      v0, 0
        
    effect_hook_default:
        lh      v0, 0x801bbf90
        
    effect_hook_end:
        lw      ra, 4(sp)
        addiu   sp, sp, 32
        jr      ra
        nop

#   HOOK: Effect (second)
#       Parameters:
#           a0 = *0x801b9130
#       Requires:
#           Set *0x801b9138 = *0x801b9130
@effect_second_hook:
        addiu   sp, sp, -16
        sw      ra, 4(sp)

        lbu     t0, @address_is_skipping_effect
        nop
        beq     t0, zero, effect_second_hook_end
        nop
        
        sb      zero, @address_is_skipping_effect
        
        lui     t0, 0x801c
        sh      zero, -0x6ed0(t0)   #   0x801b9130
        
        jal     @effect_visual_reset
        nop
        
        li      a0, 0
        
    effect_second_hook_end:
        lw      ra, 4(sp)
        addiu   sp, sp, 16
        sw      a0, 0x801b9138      #   *0x801b9138 = *0x801b9130
        jr      ra
        nop
        
#   HOOK: Effect targets
@effect_targets_hook:
        lbu     t0, @address_is_skipping_effect_targets
        nop
        beq     t0, zero, effect_targets_hook_end
        nop
        
        sb      zero, @address_is_skipping_effect_targets
        sh      zero, 0x801b9130
        
    effect_targets_hook_end:
        j       0x80077738
        nop

#   ROUTINE: Effect visual reset
#   Resets visuals to the way they were before the effect was started.
@effect_visual_reset:
        addiu   sp, sp, -32
        sw      ra, 4(sp)

        #   ColorField(4, 0, 0, 0, 0)               // Old: ColorField(4, 1, 0, 0, 0)
        li      a0, 4
        li      a1, 0                               #   NumFrames = 0 for immediate effect; used to be 1
        li      a2, 0
        li      a3, 0
        jal     @color_field
        sw      zero, 16(sp)
        
        #   ColorBackgroundTiles_Set1(4, 2, 0, 0, 0)
        li      a0, 4
        li      a1, 2
        li      a2, 0
        li      a3, 0
        jal     @color_background_tiles_set_1
        sw      zero, 16(sp)
        
        #   ColorBackgroundTiles_Set2(4, 2, 0, 0, 0)
        li      a0, 4
        li      a1, 2
        li      a2, 0
        li      a3, 0
        jal     @color_background_tiles_set_2
        sw      zero, 16(sp)
        
        #   ColorUnits([4, 1, 4, 0, 0, 0, 1])       // Could try changing last parameter to 0 for immediate effect
        addiu   a0, sp, 16
        li      t0, 4
        li      t1, 1
        sb      t0, 0(a0)
        sb      t1, 1(a0)
        sb      t0, 2(a0)
        sb      zero, 3(a0)
        sb      zero, 4(a0)
        sb      zero, 5(a0)
        jal     @color_units
        sb      t1, 6(a0)
        
        #   Reset camera zoom
        #0x801b8ad8  mult operand values 1?
        #0x801b8ae8  mult operand values 2?
        #0x801b8af8  current values?
        #0x801b8b08  setback values?
        lui     t0, 0x801c
        lw      t1, -0x74f8(t0)     #   0x801b8b08
        lw      t2, -0x74f4(t0)     #   0x801b8b0c
        lw      t3, -0x74f0(t0)     #   0x801b8b10
        sw      t1, -0x7528(t0)     #   0x801b8ad8
        sw      t2, -0x7524(t0)     #   0x801b8adc
        sw      t3, -0x7520(t0)     #   0x801b8ae0
        sw      t1, -0x7518(t0)     #   0x801b8ae8
        sw      t2, -0x7514(t0)     #   0x801b8aec
        sw      t3, -0x7510(t0)     #   0x801b8af0
        #sw      t1, -0x7508(t0)     #   0x801b8af8
        #sw      t2, -0x7504(t0)     #   0x801b8afc
        #sw      t3, -0x7500(t0)     #   0x801b8b00
        
        lw      ra, 4(sp)
        addiu   sp, sp, 32
        jr      ra
        nop
        
#   TAIL: Play effect
@play_effect_tail:
        sb      zero, @address_is_skipping_effect_targets
        jr      ra
        nop

#   HOOK: Show graphic (Chapter End)
#       Skip the Show Graphic command for Chapter End images if the event is being skipped.
#       Parameters:
#           v0 = continueLoop value (1 = true, 0 = false)
#       Returns:
#           v0 = New continueLoop value  (1 = true, 0 = false)
#       Requires:
#           Set *(t0 + 12) = s3
@show_graphic_chapter_end_hook:
        lbu     t1, @address_event_skip_type
        nop
        beq     t1, zero, show_graphic_chapter_end_hook_default
        nop
        
        li      v0, 0
        
    show_graphic_chapter_end_hook_default:    
        jr      ra
        sw      s3, 12(t0)

#   HOOK: Show graphic (Gameover)
#       Skip the Show Graphic command for the Gameover image if the event is being skipped.
#       Parameters:
#           t0 = Sentinel count value for loop
#           s0 = Count value (Default check for continueLoop is (s0 < t0))
#       Returns:
#           v0 = New continueLoop value  (1 = true, 0 = false)
@show_graphic_gameover_hook:
        lbu     t1, @address_event_skip_type
        nop
        beq     t1, zero, show_graphic_gameover_hook_default
        nop
        
        j       show_graphic_gameover_hook_end
        li      v0, 0

    show_graphic_gameover_hook_default:
        slt     v0, s0, t0
        
    show_graphic_gameover_hook_end:
        jr      ra
        nop

#   HOOK: Show graphic (Chapter Start)
#       Skip the Show Graphic command for the Chapter Start image if the event is being skipped.
#       Parameters:
#           t0 = Sentinel count value for loop
#           s1 = Count value (Default check for continueLoop is (s1 < t0))
#       Returns:
#           v0 = New continueLoop value  (1 = true, 0 = false)
@show_graphic_chapter_start_hook:
        lbu     t1, @address_event_skip_type
        nop
        beq     t1, zero, show_graphic_chapter_start_hook_default
        nop
        
        j       show_graphic_chapter_start_hook_end
        li      v0, 0

    show_graphic_chapter_start_hook_default:
        slt     v0, s1, t0
        
    show_graphic_chapter_start_hook_end:
        jr      ra
        nop

#   HOOK: Sprite move
#       If normal skipping the event (with skip type = 1), set off the sprite move command by switching threads before continuing the loop (if threads are filling up).
@sprite_move_event_instruction_hook:
        lbu     t0, @address_event_skip_type
        li      t1, 1
        bne     t0, t1, _sprite_move_event_instruction_hook_end
        nop
        
        jal     @switch_to_next_thread
        nop
        
    _sprite_move_event_instruction_hook_end:
        j       @address_event_instruction_skip_destination
        nop

#   HOOK: Wait Value
#       If skipping the event, break out of WaitValue.
@wait_value_hook:
        addiu   sp, sp, -16
        sw      s0, 4(sp)

        move    s0, ra
        jal     @switch_to_next_thread
        nop
        move    ra, s0
        
        #   If event isn't being skipped and Start isn't pressed, use the default case
        lw      t1, @address_controller_input
        lbu     t0, @address_event_skip_type
        andi    t1, t1, %start_button_flag
        or      t0, t0, t1
        beq     t0, zero, wait_value_hook_end
        nop
        
        #   Otherwise, break out of the WaitValue loop.
        la      ra, @address_event_instruction_wait_value_skip_destination
        
    wait_value_hook_end:
        lw      s0, 4(sp)
        addiu   sp, sp, 16
        jr      ra
        nop

#   HOOK: Reveal
#       If skipping the event, break out of Reveal.
#       Parameters:
#           v0 = Current Reveal value (Stops at 0) (Set value = 0 if skipping)
#       Preserve: v0 (if not skipping)
@reveal_hook:
        lbu     t0, @address_event_skip_type
        nop
        beq     t0, zero, reveal_hook_end
        nop
        
        li      v0, 0
        
    reveal_hook_end:
        jr      ra
        nop
    
#   TAIL: Post-battle
@post_battle_tail:
        sb      zero, @address_is_mid_battle
        jr      ra
        nop
        