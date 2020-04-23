<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/digest">
    <html>
      <head>
        <title>FFTPatcher changes report</title>
        <style type="text/css">
          body
          {
          background-color: white;
          }
          table.differencesTable td.fire
          {
          background-color: red;
          color: white;
          }
          table.differencesTable td.lightning
          {
          background-color: purple;
          color: white;
          }
          table.differencesTable td.ice
          {
          background-color: lightcyan;
          color: black;
          }
          table.differencesTable td.wind
          {
          background-color: yellow;
          color: black;
          }
          table.differencesTable td.earth
          {
          background-color: green;
          color: white;
          }
          table.differencesTable td.water
          {
          background-color: lightblue;
          color: black;
          }
          table.differencesTable td.holy
          {
          background-color: white;
          color: black;
          }
          table.differencesTable td.dark
          {
          background-color: black;
          color: white;
          }
          table.differencesTable {
          border-width: 2px;
          border-spacing: 5px;
          border-style: outset;
          border-color: #0000ff;
          border-collapse: collapse;
          background-color: #ffffff;
          table-layout: fixed;
          }
          table.differencesTable th {
          border-width: thin;
          padding: 5px;
          border-style: dashed;
          border-color: #808080;
          background-color: #fffafa;
          width: 100px;
          }
          table.differencesTable td {
          border-width: thin;
          padding: 5px;
          border-style: dashed;
          border-color: #808080;
          text-align: center;
          background-color: #fffafa;
          width: 200px;
          }
        </style>
      </head>
      <body>
        <xsl:apply-templates select="AllAbilities"/>
        <xsl:apply-templates select="AllItems"/>
        <xsl:apply-templates select="AllItemAttributes"/>
        <xsl:apply-templates select="AllJobs"/>
        <xsl:apply-templates select="AllJobLevels"/>
        <xsl:apply-templates select="AllSkillSets"/>
        <xsl:apply-templates select="AllMonsterSkills"/>
        <xsl:apply-templates select="AllActionMenus"/>
        <xsl:apply-templates select="AllStatusAttributes"/>
        <xsl:apply-templates select="AllInflictStatuses"/>
        <xsl:apply-templates select="AllPoachProbabilities"/>
        <xsl:apply-templates select="AllENTDs"/>
        <xsl:apply-templates select="AllMoveFindItems"/>
      </body>
    </html>
  </xsl:template>
  <xsl:template match="/digest/AllAbilities">
    <h1>Abilities</h1>
    <xsl:for-each select="Ability">
      <h2>
        0x<xsl:value-of select="@value"/><xsl:text xml:space="preserve"> </xsl:text><xsl:value-of select="@name"/>
      </h2>
      <table class="differencesTable">
        <tr>
          <th>Setting</th>
          <th>Default</th>
          <th>New value</th>
        </tr>
        <xsl:for-each select="JPCost|LearnRate|AbilityType|LearnWithJP|Action|LearnOnHit|Blank1|Unknown1|Unknown2|Unknown3|Blank2|Blank3|Blank4|Blank5|Unknown4">
          <tr>
            <td>
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
        <xsl:for-each select="AIFlags/HP|AIFlags/MP|AIFlags/CancelStatus|AIFlags/AddStatus|AIFlags/Stats|AIFlags/Unequip|AIFlags/TargetEnemies|AIFlags/TargetAllies|AIFlags/LineOfSight|AIFlags/Reflectable|AIFlags/UndeadReverse|AIFlags/Unknown1|AIFlags/AllowRandomly|AIFlags/Unknown2|AIFlags/Unknown3|AIFlags/Silence|AIFlags/Blank|AIFlags/Unknown4|AIFlags/Unknown5|AIFlags/Unknown6|AIFlags/Unknown7|AIFlags/Unknown8|AIFlags/Unknown9|AIFlags/Unknown10">
          <tr>
            <td>
              AI:
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
        <xsl:for-each select="Effect">
          <tr>
            <td>
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
        <xsl:for-each select="AbilityAttributes/*">
          <xsl:apply-templates select=".">
            <xsl:with-param name="prepend">Elements</xsl:with-param>
          </xsl:apply-templates>
        </xsl:for-each>
        <xsl:for-each select="InflictStatusDescription|CastSpell">
          <tr>
            <td>
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
        <xsl:for-each select="ItemOffset">
          <tr>
            <td>
              Chemist Item:
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
        <xsl:for-each select="Throwing">
          <tr>
            <td>
              Throwing:
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
        <xsl:for-each select="JumpHorizontal|JumpVertical">
          <tr>
            <td>
              Jumping:
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
        <xsl:for-each select="ChargeCT|ChargeBonus">
          <tr>
            <td>
              Charging:
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
        <xsl:for-each select="ArithmetickSkill">
          <tr>
            <td>
              Arithmeticks:
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
        <xsl:for-each select="OtherID">
          <tr>
            <td>
              Other:
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="/digest/AllItems">
    <h1>Items</h1>
    <xsl:for-each select="Weapon|Shield|Armor|Accessory|ChemistItem">
      <h2>
        <xsl:value-of select="@name"/>
      </h2>
      <table class="differencesTable">
        <tr>
          <th>Setting</th>
          <th>Default</th>
          <th>New value</th>
        </tr>
        <xsl:for-each select="Palette|Graphic|EnemyLevel|ItemType|SIA|Price|ShopAvailability|Weapon|Shield|Head|Body|Accessory|Blank1|Rare|Blank2|SecondTableId">
          <tr>
            <td>
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
        <xsl:if test="name(.)=&apos;Weapon&apos;">
          <xsl:for-each select="Range|Formula|WeaponPower|EvadePercentage|InflictStatus|Striking|Lunking|Direct|Arc|TwoSwords|TwoHands|Blank|Force2Hands">
            <tr>
              <td>
                Weapon:
                <xsl:value-of select="name(.)"/>
              </td>
              <td>
                <xsl:value-of select="@default"/>
              </td>
              <td>
                <xsl:value-of select="@value"/>
              </td>
            </tr>
          </xsl:for-each>
          <xsl:for-each select="Elements">
            <xsl:call-template name="elements">
              <xsl:with-param name="prepend">Weapon: Elements</xsl:with-param>
            </xsl:call-template>
          </xsl:for-each>
          <xsl:for-each select="InflictStatusDescription|CastSpell">
            <tr>
              <td>
                <xsl:value-of select="name(.)"/>
              </td>
              <td>
                <xsl:value-of select="@default"/>
              </td>
              <td>
                <xsl:value-of select="@value"/>
              </td>
            </tr>
          </xsl:for-each>
        </xsl:if>
        <xsl:if test="name(.)=&apos;Shield&apos;">
          <xsl:for-each select="PhysicalBlockRate|MagicBlockRate">
            <tr>
              <td>
                Shield:
                <xsl:value-of select="name(.)"/>
              </td>
              <td>
                <xsl:value-of select="@default"/>
              </td>
              <td>
                <xsl:value-of select="@value"/>
              </td>
            </tr>
          </xsl:for-each>
        </xsl:if>
        <xsl:if test="name(.)=&apos;Armor&apos;">
          <xsl:for-each select="HPBonus|MPBonus">
            <tr>
              <td>
                Armor:
                <xsl:value-of select="name(.)"/>
              </td>
              <td>
                <xsl:value-of select="@default"/>
              </td>
              <td>
                <xsl:value-of select="@value"/>
              </td>
            </tr>
          </xsl:for-each>
        </xsl:if>
        <xsl:if test="name(.)=&apos;Accessory&apos;">
          <xsl:for-each select="PhysicalEvade|MagicEvade">
            <tr>
              <td>
                Accessory:
                <xsl:value-of select="name(.)"/>
              </td>
              <td>
                <xsl:value-of select="@default"/>
              </td>
              <td>
                <xsl:value-of select="@value"/>
              </td>
            </tr>
          </xsl:for-each>
        </xsl:if>
        <xsl:if test="name(.)=&apos;ChemistItem&apos;">
          <xsl:for-each select="Formula|X">
            <tr>
              <td>
                Chemist Item:
                <xsl:value-of select="name(.)"/>
              </td>
              <td>
                <xsl:value-of select="@default"/>
              </td>
              <td>
                <xsl:value-of select="@value"/>
              </td>
            </tr>
          </xsl:for-each>
        </xsl:if>
      </table>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="/digest/AllItemAttributes">
    <h1>Item Attribute</h1>
    <xsl:for-each select="ItemAttributes">
      <h2>
        0x<xsl:value-of select="@value"/>
      </h2>
      <ul>
        <li>
          Corresponding items: <xsl:value-of select="CorrespondingItems"/>
        </li>
      </ul>
      <table class="differencesTable">
        <tr>
          <th>Setting</th>
          <th>Default</th>
          <th>New value</th>
        </tr>
        <xsl:for-each select="PA|MA|Speed|Move|Jump">
          <tr>
            <td>
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
        <xsl:for-each select="Absorb|Cancel|Half|Weak|Strong">
          <xsl:call-template name="elements">
            <xsl:with-param name="prepend">
              <xsl:value-of select="name(.)"/> Elements
            </xsl:with-param>
          </xsl:call-template>
        </xsl:for-each>
        <xsl:for-each select="PermanentStatuses|StatusImmunity|StartingStatuses">
          <xsl:call-template name="statuses">
            <xsl:with-param name="prepend">
              <xsl:value-of select="name(.)"/>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:for-each>
      </table>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="/digest/AllJobs">
    <h1>Jobs</h1>
    <xsl:for-each select="Job">
      <h2>
        0x<xsl:value-of select="@value"/><xsl:text xml:space="preserve"> </xsl:text><xsl:value-of select="@name"/>
      </h2>
      <table class="differencesTable">
        <tr>
          <th>Setting</th>
          <th>Default</th>
          <th>New value</th>
        </tr>
        <xsl:for-each select="SkillSet|HPConstant|HPMultiplier|MPConstant|MPMultiplier|SpeedConstant|SpeedMultiplier|PAConstant|PAMultiplier|MAConstant|MAMultiplier|Move|Jump|CEvade|MPortrait|MPalette|MGraphic|InnateA|InnateB|InnateC|InnateD">
          <tr>
            <td>
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
        <xsl:for-each select="AbsorbElement|CancelElement|HalfElement|WeakElement">
          <xsl:call-template name="elements">
            <xsl:with-param name="prepend">
              <xsl:value-of select="name(.)"/>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:for-each>

        <xsl:for-each select="Equipment">
          <xsl:call-template name="equipment">
            <xsl:with-param name="prepend">
              <xsl:value-of select="name(.)"/>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:for-each>

        <xsl:for-each select="PermanentStatus|StartingStatus|StatusImmunity">
          <xsl:call-template name="statuses">
            <xsl:with-param name="prepend">
              <xsl:value-of select="name(.)"/>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:for-each>

      </table>
    </xsl:for-each>
  </xsl:template>
  <!--	<h1>TODO: Job levels</h1> -->
  <xsl:template match="/digest/AllSkillSets">
    <h1>Skillsets</h1>
    <xsl:for-each select="SkillSet">
      <h2>
        0x<xsl:value-of select="@value"/><xsl:text xml:space="preserve"> </xsl:text><xsl:value-of select="@name"/>
      </h2>
      <ul>
        <li>
          Corresponding jobs: <xsl:value-of select="CorrespondingJobs"/>
        </li>
      </ul>
      <table class="differencesTable">
        <tr>
          <th>Setting</th>
          <th>Default</th>
          <th>New value</th>
        </tr>
        <xsl:for-each select="Action1|Action2|Action3|Action4|Action5|Action6|Action7|Action8|Action9|Action10|Action11|Action12|Action13|Action14|Action15|Action16|TheRest1|TheRest2|TheRest3|TheRest4|TheRest5|TheRest6">
          <tr>
            <td>
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="/digest/AllMonsterSkills">
    <h1>Monster Skills</h1>
    <xsl:for-each select="MonsterSkill">
      <h2>
        0x<xsl:value-of select="@value"/><xsl:text xml:space="preserve"> </xsl:text><xsl:value-of select="@name"/>
      </h2>
      <table class="differencesTable">
        <tr>
          <th>Setting</th>
          <th>Default</th>
          <th>New value</th>
        </tr>
        <xsl:for-each select="Ability1|Ability2|Ability3|Beastmaster">
          <tr>
            <td>
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="/digest/AllActionMenus">
    <h1>Action Menus</h1>
    <xsl:for-each select="ActionMenu">
      <h2>
        0x<xsl:value-of select="@value"/><xsl:text xml:space="preserve"> </xsl:text><xsl:value-of select="@name"/>
      </h2>
      <table class="differencesTable">
        <tr>
          <th>Setting</th>
          <th>Default</th>
          <th>New value</th>
        </tr>
        <xsl:for-each select="MenuAction">
          <tr>
            <td>
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="/digest/AllStatusAttributes">
    <h1>Status Attributes</h1>
    <xsl:for-each select="StatusAttribute">
      <h2>
        <xsl:value-of select="@name"/>
      </h2>
      <table class="differencesTable">
        <tr>
          <th>Setting</th>
          <th>Default</th>
          <th>New value</th>
        </tr>
        <xsl:for-each select="Blank1|Blank2|Order|CT|FreezeCT|Unknown1|Unknown2|Unknown3|CancelWhenHit|Unknown5|Unknown6|CountsAsKO|CanReact|Blank|IgnoreAttacks|Unknown7|Unknown8|Unknown9|CancelledByImmortal|LowerTargetPriority">
          <tr>
            <td>
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
        <xsl:for-each select="Cancels|CantStackOn">
          <xsl:call-template name="statuses">
            <xsl:with-param name="prepend">
              <xsl:value-of select="name(.)"/> Status
            </xsl:with-param>
          </xsl:call-template>
        </xsl:for-each>

      </table>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="/digest/AllInflictStatuses">
    <h1>Inflict Statuses</h1>
    <xsl:for-each select="InflictStatus">
      <h2>
        0x<xsl:value-of select="@value"/>
      </h2>
      <ul>
        <li>
          Corresponding abilities: <xsl:value-of select="CorrespondingAbilities"/>
        </li>
        <li>
          Corresponding weapons: <xsl:value-of select="CorrespondingWeapons"/>
        </li>
        <li>
          Corresponding chemist items: <xsl:value-of select="CorrespondingChemistItems"/>
        </li>
      </ul>
      <table class="differencesTable">
        <tr>
          <th>Setting</th>
          <th>Default</th>
          <th>New value</th>
        </tr>
        <xsl:for-each select="AllOrNothing|Random|Separate|Cancel|Blank1|Blank2|Blank3|Blank4">
          <tr>
            <td>
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
        <xsl:for-each select="Statuses">
          <xsl:call-template name="statuses">
            <xsl:with-param name="prepend">
              <xsl:value-of select="name(.)"/> Status
            </xsl:with-param>
          </xsl:call-template>
        </xsl:for-each>
      </table>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="/digest/AllPoachProbabilities">
    <h1>Poaching</h1>
    <xsl:for-each select="PoachProbability">
      <h2>
        <xsl:value-of select="@name"/>
      </h2>
      <table class="differencesTable">
        <tr>
          <th>Setting</th>
          <th>Default</th>
          <th>New value</th>
        </tr>
        <xsl:for-each select="Common|Uncommon">
          <tr>
            <td>
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="/digest/AllMoveFindItems">
    <h1>Poaching</h1>
    <xsl:for-each select="MapMoveFindItems">
      <h2>
        <xsl:value-of select="@name"/>
      </h2>
      <table class="differencesTable">
        <tr>
          <th>Setting</th>
          <th>Default</th>
          <th>New value</th>
        </tr>
        <xsl:for-each select="X|Y|CommonItem|RareItem|Unknown1|Unknown2|Unknown3|Unknown4|SteelNeedle|SleepingGas|Deathtrap|Degenerator">
          <tr>
            <td>
              <xsl:value-of select="name(.)"/>
            </td>
            <td>
              <xsl:value-of select="@default"/>
            </td>
            <td>
              <xsl:value-of select="@value"/>
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="/digest/AllENTDs">
    <h1>Events</h1>
    <xsl:for-each select="Event">
      <h2>
        <xsl:value-of select="@value"/>
      </h2>
      <xsl:for-each select="EventUnit">
        <h3>
          Unit
          <xsl:value-of select="@value"/>
        </h3>
        <table class="differencesTable">
          <tr>
            <th>Setting</th>
            <th>Default</th>
            <th>New value</th>
          </tr>
          <xsl:for-each select="SpriteSet|SpecialName|Month|Day|Job|Level|Faith|Bravery|Palette|UnitID|X|Y|PrerequisiteJob|PrerequisiteJobLevel|FacingDirection|Target|SkillSet|SecondaryAction|Reaction|Support|Movement|RightHand|LeftHand|Head|Body|Accessory|BonusMoney|WarTrophy|Male|Female|Monster|JoinAfterEvent|LoadFormation|ZodiacMonster|Blank2|SaveFormation|AlwaysPresent|RandomlyPresent|Control|Immortal|Blank6|Blank7|Unknown2|Unknown6|Unknown7|Unknown8|Unknown10|Unknown11|Unknown12">
            <tr>
              <td>
                <xsl:value-of select="name(.)"/>
              </td>
              <td>
                <xsl:value-of select="@default"/>
              </td>
              <td>
                <xsl:value-of select="@value"/>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </xsl:for-each>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="/digest/AllAbilities/Ability/AbilityAttributes/*">
    <tr>
      <td>
        Attributes:
        <xsl:value-of select="name(.)"/>
      </td>
      <td>
        <xsl:value-of select="@default"/>
      </td>
      <td>
        <xsl:value-of select="@value"/>
      </td>
    </tr>
  </xsl:template>
  <xsl:template name="elements" match="/digest/AllAbilities/Ability/AbilityAttributes/Elements">
    <xsl:param name="prepend" />
    <xsl:if test="Fire">
      <tr>
        <td class="fire">
          <xsl:value-of select="$prepend"/>: Fire
        </td>
        <td class="fire">
          <xsl:value-of select="Fire/@default"/>
        </td>
        <td class="fire">
          <xsl:value-of select="Fire/@value"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="Lightning">
      <tr>
        <td class="lightning">
          <xsl:value-of select="$prepend"/>: Lightning
        </td>
        <td class="lightning">
          <xsl:value-of select="Lightning/@default"/>
        </td>
        <td class="lightning">
          <xsl:value-of select="Lightning/@value"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="Ice">
      <tr>
        <td class="ice">
          <xsl:value-of select="$prepend"/>: Ice
        </td>
        <td class="ice">
          <xsl:value-of select="Ice/@default"/>
        </td>
        <td class="ice">
          <xsl:value-of select="Ice/@value"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="Wind">
      <tr>
        <td class="wind">
          <xsl:value-of select="$prepend"/>: Wind
        </td>
        <td class="wind">
          <xsl:value-of select="Wind/@default"/>
        </td>
        <td class="wind">
          <xsl:value-of select="Wind/@value"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="Earth">
      <tr>
        <td class="earth">
          <xsl:value-of select="$prepend"/>: Earth
        </td>
        <td class="earth">
          <xsl:value-of select="Earth/@default"/>
        </td>
        <td class="earth">
          <xsl:value-of select="Earth/@value"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="Water">
      <tr>
        <td class="water">
          <xsl:value-of select="$prepend"/>: Water
        </td>
        <td class="water">
          <xsl:value-of select="Water/@default"/>
        </td>
        <td class="water">
          <xsl:value-of select="Water/@value"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="Holy">
      <tr>
        <td class="holy">
          <xsl:value-of select="$prepend"/>: Holy
        </td>
        <td class="holy">
          <xsl:value-of select="Holy/@default"/>
        </td>
        <td class="holy">
          <xsl:value-of select="Holy/@value"/>
        </td>
      </tr>
    </xsl:if>
    <xsl:if test="Dark">
      <tr>
        <td class="dark">
          <xsl:value-of select="$prepend"/>: Dark
        </td>
        <td class="dark">
          <xsl:value-of select="Dark/@default"/>
        </td>
        <td class="dark">
          <xsl:value-of select="Dark/@value"/>
        </td>
      </tr>
    </xsl:if>

  </xsl:template>

  <xsl:template name="statuses">
    <xsl:param name="prepend" />
    <xsl:for-each select="NoEffect|Crystal|Dead|Undead|Charging|Jump|Defending|Performing|Petrify|Invite|Darkness|Confusion|Silence|BloodSuck|DarkEvilLooking|Treasure|Oil|Float|Reraise|Transparent|Berserk|Chicken|Frog|Critical|Poison|Regen|Protect|Shell|Haste|Slow|Stop|Wall|Faith|Innocent|Charm|Sleep|DontMove|DontAct|Reflect|DeathSentence">
      <tr>
        <td>
          <xsl:value-of select="$prepend"/>: <xsl:value-of select="name(.)"/>
        </td>
        <td>
          <xsl:value-of select="@default"/>
        </td>
        <td>
          <xsl:value-of select="@value"/>
        </td>
      </tr>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="equipment">
    <xsl:param name="prepend" />
    <xsl:for-each select="Unused|Knife|NinjaBlade|Sword|KnightsSword|Katana|Axe|Rod|Staff|Flail|Gun|Crossbow|Bow|Instrument|Book|Polearm|Pole|Bag|Cloth|Shield|Helmet|Hat|HairAdornment|Armor|Clothing|Robe|Shoes|Armguard|Ring|Armlet|Cloak|Perfume|Unknown1|Unknown2|Unknown3|FellSword|LipRouge|Unknown6|Unknown7|Unknown8">
      <tr>
        <td>
          <xsl:value-of select="$prepend"/>: <xsl:value-of select="name(.)"/>
        </td>
        <td>
          <xsl:value-of select="@default"/>
        </td>
        <td>
          <xsl:value-of select="@value"/>
        </td>
      </tr>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>