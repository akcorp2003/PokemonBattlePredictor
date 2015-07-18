<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BattleSetup
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Pokemon_Name = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.InsertPokemon = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TeamBlue_List = New System.Windows.Forms.ListView()
        Me.Pokemon = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.HP = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ATK = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.DEF = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SpATK = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SpDEF = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Speed = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Ability = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.TeamRed_List = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Worker_FetchMove = New System.ComponentModel.BackgroundWorker()
        Me.Initiate_Build = New System.Windows.Forms.Button()
        Me.Initiate_movebuild = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Pokemon_Name
        '
        Me.Pokemon_Name.AutoCompleteCustomSource.AddRange(New String() {"Bulbasaur ", "Ivysaur ", "Venusaur ", "Charmander ", "Charmeleon ", "Charizard ", "Squirtle ", "Wartortle ", "Blastoise ", "Caterpie ", "Metapod ", "Butterfree ", "Weedle ", "Kakuna ", "Beedrill ", "Pidgey ", "Pidgeotto ", "Pidgeot ", "Rattata ", "Raticate ", "Spearow ", "Fearow ", "Ekans ", "Arbok ", "Pikachu ", "Raichu ", "Sandshrew ", "Sandslash ", "Nidoran-f ", "Nidorina ", "Nidoqueen ", "Nidoran-m ", "Nidorino ", "Nidoking ", "Clefairy ", "Clefable ", "Vulpix ", "Ninetales ", "Jigglypuff ", "Wigglytuff ", "Zubat ", "Golbat ", "Oddish ", "Gloom ", "Vileplume ", "Paras ", "Parasect ", "Venonat ", "Venomoth ", "Diglett ", "Dugtrio ", "Meowth ", "Persian ", "Psyduck ", "Golduck ", "Mankey ", "Primeape ", "Growlithe ", "Arcanine ", "Poliwag ", "Poliwhirl ", "Poliwrath ", "Abra ", "Kadabra ", "Alakazam ", "Machop ", "Machoke ", "Machamp ", "Bellsprout ", "Weepinbell ", "Victreebel ", "Tentacool ", "Tentacruel ", "Geodude ", "Graveler ", "Golem ", "Ponyta ", "Rapidash ", "Slowpoke ", "Slowbro ", "Magnemite ", "Magneton ", "Farfetch'd ", "Doduo ", "Dodrio ", "Seel ", "Dewgong ", "Grimer ", "Muk ", "Shellder ", "Cloyster ", "Gastly ", "Haunter ", "Gengar ", "Onix ", "Drowzee ", "Hypno ", "Krabby ", "Kingler ", "Voltorb ", "Electrode ", "Exeggcute ", "Exeggutor ", "Cubone ", "Marowak ", "Hitmonlee ", "Hitmonchan ", "Lickitung ", "Koffing ", "Weezing ", "Rhyhorn ", "Rhydon ", "Chansey ", "Tangela ", "Kangaskhan ", "Horsea ", "Seadra ", "Goldeen ", "Seaking ", "Staryu ", "Starmie ", "Mr. Mime ", "Scyther ", "Jynx ", "Electabuzz ", "Magmar ", "Pinsir ", "Tauros ", "Magikarp ", "Gyarados ", "Lapras ", "Ditto ", "Eevee ", "Vaporeon ", "Jolteon ", "Flareon ", "Porygon ", "Omanyte ", "Omastar ", "Kabuto ", "Kabutops ", "Aerodactyl ", "Snorlax ", "Articuno ", "Zapdos ", "Moltres ", "Dratini ", "Dragonair ", "Dragonite ", "Mewtwo ", "Mew ", "Chikorita ", "Bayleef ", "Meganium ", "Cyndaquil ", "Quilava ", "Typhlosion ", "Totodile ", "Croconaw ", "Feraligatr ", "Sentret ", "Furret ", "Hoothoot ", "Noctowl ", "Ledyba ", "Ledian ", "Spinarak ", "Ariados ", "Crobat ", "Chinchou ", "Lanturn ", "Pichu ", "Cleffa ", "Igglybuff ", "Togepi ", "Togetic ", "Natu ", "Xatu ", "Mareep ", "Flaaffy ", "Ampharos ", "Bellossom ", "Marill ", "Azumarill ", "Sudowoodo ", "Politoed ", "Hoppip ", "Skiploom ", "Jumpluff ", "Aipom ", "Sunkern ", "Sunflora ", "Yanma ", "Wooper ", "Quagsire ", "Espeon ", "Umbreon ", "Murkrow ", "Slowking ", "Misdreavus ", "Unown ", "Wobbuffet ", "Girafarig ", "Pineco ", "Forretress ", "Dunsparce ", "Gligar ", "Steelix ", "Snubbull ", "Granbull ", "Qwilfish ", "Scizor ", "Shuckle ", "Heracross ", "Sneasel ", "Teddiursa ", "Ursaring ", "Slugma ", "Magcargo ", "Swinub ", "Piloswine ", "Corsola ", "Remoraid ", "Octillery ", "Delibird ", "Mantine ", "Skarmory ", "Houndour ", "Houndoom ", "Kingdra ", "Phanpy ", "Donphan ", "Porygon2 ", "Stantler ", "Smeargle ", "Tyrogue ", "Hitmontop ", "Smoochum ", "Elekid ", "Magby ", "Miltank ", "Blissey ", "Raikou ", "Entei ", "Suicune ", "Larvitar ", "Pupitar ", "Tyranitar ", "Lugia ", "Ho-oh ", "Celebi ", "Treecko ", "Grovyle ", "Sceptile ", "Torchic ", "Combusken ", "Blaziken ", "Mudkip ", "Marshtomp ", "Swampert ", "Poochyena ", "Mightyena ", "Zigzagoon ", "Linoone ", "Wurmple ", "Silcoon ", "Beautifly ", "Cascoon ", "Dustox ", "Lotad ", "Lombre ", "Ludicolo ", "Seedot ", "Nuzleaf ", "Shiftry ", "Taillow ", "Swellow ", "Wingull ", "Pelipper ", "Ralts ", "Kirlia ", "Gardevoir ", "Surskit ", "Masquerain ", "Shroomish ", "Breloom ", "Slakoth ", "Vigoroth ", "Slaking ", "Nincada ", "Ninjask ", "Shedinja ", "Whismur ", "Loudred ", "Exploud ", "Makuhita ", "Hariyama ", "Azurill ", "Nosepass ", "Skitty ", "Delcatty ", "Sableye ", "Mawile ", "Aron ", "Lairon ", "Aggron ", "Meditite ", "Medicham ", "Electrike ", "Manectric ", "Plusle ", "Minun ", "Volbeat ", "Illumise ", "Roselia ", "Gulpin ", "Swalot ", "Carvanha ", "Sharpedo ", "Wailmer ", "Wailord ", "Numel ", "Camerupt ", "Torkoal ", "Spoink ", "Grumpig ", "Spinda ", "Trapinch ", "Vibrava ", "Flygon ", "Cacnea ", "Cacturne ", "Swablu ", "Altaria ", "Zangoose ", "Seviper ", "Lunatone ", "Solrock ", "Barboach ", "Whiscash ", "Corphish ", "Crawdaunt ", "Baltoy ", "Claydol ", "Lileep ", "Cradily ", "Anorith ", "Armaldo ", "Feebas ", "Milotic ", "Castform ", "Kecleon ", "Shuppet ", "Banette ", "Duskull ", "Dusclops ", "Tropius ", "Chimecho ", "Absol ", "Wynaut ", "Snorunt ", "Glalie ", "Spheal ", "Sealeo ", "Walrein ", "Clamperl ", "Huntail ", "Gorebyss ", "Relicanth ", "Luvdisc ", "Bagon ", "Shelgon ", "Salamence ", "Beldum ", "Metang ", "Metagross ", "Regirock ", "Regice ", "Registeel ", "Latias ", "Latios ", "Kyogre ", "Groudon ", "Rayquaza ", "Jirachi ", "Deoxys ", "Turtwig ", "Grotle ", "Torterra ", "Chimchar ", "Monferno ", "Infernape ", "Piplup ", "Prinplup ", "Empoleon ", "Starly ", "Staravia ", "Staraptor ", "Bidoof ", "Bibarel ", "Kricketot ", "Kricketune ", "Shinx ", "Luxio ", "Luxray ", "Budew ", "Roserade ", "Cranidos ", "Rampardos ", "Shieldon ", "Bastiodon ", "Burmy ", "Wormadam ", "Mothim ", "Combee ", "Vespiquen ", "Pachirisu ", "Buizel ", "Floatzel ", "Cherubi ", "Cherrim ", "Shellos ", "Gastrodon ", "Ambipom ", "Drifloon ", "Drifblim ", "Buneary ", "Lopunny ", "Mismagius ", "Honchkrow ", "Glameow ", "Purugly ", "Chingling ", "Stunky ", "Skuntank ", "Bronzor ", "Bronzong ", "Bonsly ", "Mime Jr. ", "Happiny ", "Chatot ", "Spiritomb ", "Gible ", "Gabite ", "Garchomp ", "Munchlax ", "Riolu ", "Lucario ", "Hippopotas ", "Hippowdon ", "Skorupi ", "Drapion ", "Croagunk ", "Toxicroak ", "Carnivine ", "Finneon ", "Lumineon ", "Mantyke ", "Snover ", "Abomasnow ", "Weavile ", "Magnezone ", "Lickilicky ", "Rhyperior ", "Tangrowth ", "Electivire ", "Magmortar ", "Togekiss ", "Yanmega ", "Leafeon ", "Glaceon ", "Gliscor ", "Mamoswine ", "Porygon-Z ", "Gallade ", "Probopass ", "Dusknoir ", "Froslass ", "Rotom ", "Uxie ", "Mesprit ", "Azelf ", "Dialga ", "Palkia ", "Heatran ", "Regigigas ", "Giratina ", "Cresselia ", "Phione ", "Manaphy ", "Darkrai ", "Shaymin ", "Arceus", "Victini", "Snivy", "Servine", "Serperior", "Tepig", "Pignite", "Emboar", "Oshawott", "Dewott", "Samurott", "Patrat", "Watchog", "Lillipup", "Herdier", "Stoutland", "Purrloin", "Liepard", "Pansage", "Simisage", "Pansear", "Simisear", "Panpour", "Simipour", "Munna", "Musharna", "Pidove", "Tranquill", "Unfezant-f", "Unfezant-m", "Blitzle", "Zebstrika", "Roggenrola", "Boldore", "Gigalith", "Woobat", "Swoobat", "Drilbur", "Excadrill", "Audino", "Timburr", "Gurdurr", "Conkeldurr", "Tympole", "Palpitoad", "Seismitoad", "Throh", "Sawk", "Sewaddle", "Swadloon", "Leavanny", "Venipede", "Whirlipede", "Scolipede", "Cottonee", "Whimsicott", "Petilil", "Lilligant", "Basculin red striped", "Basculin green striped", "Sandile", "Krokorok", "Krookodile", "Darumaka", "Darmanitan", "Darmanitan", "Maractus", "Dwebble", "Crustle", "Scraggy", "Scrafty", "Sigilyph", "Yamask", "Cofagrigus", "Tirtouga", "Carracosta", "Archen", "Acheops", "Trubbish", "Garbodor", "Zorua", "Zoroark", "Minccino", "Cinccino", "Gothita", "Gothorita", "Gothitelle", "Solosis", "Duosion", "Reuniclus", "Ducklett", "Swanna", "Vanillite", "Vanillish", "Vanilluxe", "Deerling", "Sawsbuck", "Emolga", "Karrablast", "Escavalier", "Foongus", "Amoonguss", "Frillish", "Jellicent", "Alomomola", "Joltik", "Galvantula", "Ferroseed", "Ferrothorn", "Klink", "Klang", "Klinklang", "Tyanmo", "Eelektrik", "Eelektross", "Elgyem", "Beheeyem", "Litwick", "Lampent", "Chandelure", "Axew", "Fraxure", "Haxorus", "Cubchoo", "Beartic", "Cryogonal", "Shelmet", "Accelgor", "Stunfisk", "Mienfoo", "Mienshao", "Druddigon", "Golett", "Golurk", "Pawniard", "Bisharp", "Bouffalant", "Rufflet", "Braviary", "Vullaby", "Mandibuzz", "Heatmor", "Durant", "Deino", "Zweilous", "Hydreigon", "Larvesta", "Volcarona", "Cobalion", "Terrakion", "Virizion", "Tornadus", "Thundurus", "Reshiram", "Zekrom", "Landorus", "Kyurem", "Keldeo", "Meloetta", "Meloetta", "Genesect", "Chespin", "Quilladin", "Chesnaught", "Fennekin", "Braixen", "Delphox", "Froakie", "Frogadier", "Bunnelby", "Diggersby", "Fletchling", "Fletchinder", "Talonflame", "Scatterbug", "Spewpa", "Vivillon", "Litleo", "Pyroar", "Flabebe", "Floette", "Florges", "Skiddo", "Gogoat", "Pancahm", "Pangoro", "Furfrou", "Espurr", "Meowstic", "Honedge", "Doublade", "Aegislash", "Spritzee", "Aromatisse", "Swirlix", "Slurpuff", "Inkay", "Malamar", "Binacle", "Barbaracle", "Skrelp", "Dragalge", "Greninja", "Clauncher", "Clawitzer", "Helioptile", "Heliolisk", "Tyrunt", "Tyrantrum", "Amaura", "Aurorus", "Sylveon", "Hawlucha", "Dedenne", "Carbink", "Goomy", "Sliggoo", "Goodra", "Klefki", "Phantump", "Trevenant", "Pumpkaboo", "Gourgeist", "Bergmite", "Avalugg", "Noibat", "Noivern", "Xerneas", "Yveltal", "Zygarde", "Diancie", "Hoopa", "Hoopa", "Volcanion"})
        Me.Pokemon_Name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Pokemon_Name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.Pokemon_Name.Font = New System.Drawing.Font("Lucida Bright", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Pokemon_Name.Location = New System.Drawing.Point(254, 27)
        Me.Pokemon_Name.Name = "Pokemon_Name"
        Me.Pokemon_Name.Size = New System.Drawing.Size(267, 21)
        Me.Pokemon_Name.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Comic Sans MS", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(32, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(188, 20)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Search for your Pokemon:"
        '
        'InsertPokemon
        '
        Me.InsertPokemon.Location = New System.Drawing.Point(347, 53)
        Me.InsertPokemon.Name = "InsertPokemon"
        Me.InsertPokemon.Size = New System.Drawing.Size(75, 23)
        Me.InsertPokemon.TabIndex = 2
        Me.InsertPokemon.Text = "ADD"
        Me.InsertPokemon.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Red
        Me.Button1.Font = New System.Drawing.Font("Comic Sans MS", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.Lime
        Me.Button1.Location = New System.Drawing.Point(269, 562)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(231, 52)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "LOAD MY POKEMON!"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Lucida Bright", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(32, 315)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(102, 22)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Team Red"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Lucida Bright", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Blue
        Me.Label2.Location = New System.Drawing.Point(32, 91)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(107, 22)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Team Blue"
        '
        'TeamBlue_List
        '
        Me.TeamBlue_List.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Pokemon, Me.HP, Me.ATK, Me.DEF, Me.SpATK, Me.SpDEF, Me.Speed, Me.Ability})
        Me.TeamBlue_List.Font = New System.Drawing.Font("Lucida Bright", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TeamBlue_List.Location = New System.Drawing.Point(36, 145)
        Me.TeamBlue_List.Name = "TeamBlue_List"
        Me.TeamBlue_List.Size = New System.Drawing.Size(485, 141)
        Me.TeamBlue_List.TabIndex = 12
        Me.TeamBlue_List.UseCompatibleStateImageBehavior = False
        Me.TeamBlue_List.View = System.Windows.Forms.View.Details
        '
        'Pokemon
        '
        Me.Pokemon.Text = "Pokemon"
        Me.Pokemon.Width = 94
        '
        'HP
        '
        Me.HP.Text = "HP"
        Me.HP.Width = 45
        '
        'ATK
        '
        Me.ATK.Text = "ATK"
        Me.ATK.Width = 38
        '
        'DEF
        '
        Me.DEF.Text = "DEF"
        Me.DEF.Width = 36
        '
        'SpATK
        '
        Me.SpATK.Text = "Sp. ATK"
        Me.SpATK.Width = 54
        '
        'SpDEF
        '
        Me.SpDEF.Text = "Sp. DEF"
        Me.SpDEF.Width = 52
        '
        'Speed
        '
        Me.Speed.Text = "Speed"
        '
        'Ability
        '
        Me.Ability.Text = "Ability"
        '
        'TeamRed_List
        '
        Me.TeamRed_List.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader7, Me.ColumnHeader8})
        Me.TeamRed_List.Font = New System.Drawing.Font("Lucida Bright", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TeamRed_List.Location = New System.Drawing.Point(36, 361)
        Me.TeamRed_List.Name = "TeamRed_List"
        Me.TeamRed_List.Size = New System.Drawing.Size(485, 141)
        Me.TeamRed_List.TabIndex = 13
        Me.TeamRed_List.UseCompatibleStateImageBehavior = False
        Me.TeamRed_List.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Pokemon"
        Me.ColumnHeader1.Width = 94
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "HP"
        Me.ColumnHeader2.Width = 45
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "ATK"
        Me.ColumnHeader3.Width = 38
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "DEF"
        Me.ColumnHeader4.Width = 36
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Sp. ATK"
        Me.ColumnHeader5.Width = 54
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Sp. DEF"
        Me.ColumnHeader6.Width = 52
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Speed"
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Ability"
        '
        'Initiate_Build
        '
        Me.Initiate_Build.Location = New System.Drawing.Point(595, 27)
        Me.Initiate_Build.Name = "Initiate_Build"
        Me.Initiate_Build.Size = New System.Drawing.Size(155, 23)
        Me.Initiate_Build.TabIndex = 14
        Me.Initiate_Build.Text = "Build Pokemon Dictionary"
        Me.Initiate_Build.UseVisualStyleBackColor = True
        '
        'Initiate_movebuild
        '
        Me.Initiate_movebuild.Location = New System.Drawing.Point(595, 93)
        Me.Initiate_movebuild.Name = "Initiate_movebuild"
        Me.Initiate_movebuild.Size = New System.Drawing.Size(155, 23)
        Me.Initiate_movebuild.TabIndex = 15
        Me.Initiate_movebuild.Text = "Build Move Dictionary"
        Me.Initiate_movebuild.UseVisualStyleBackColor = True
        '
        'BattleSetup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(762, 626)
        Me.Controls.Add(Me.Initiate_movebuild)
        Me.Controls.Add(Me.Initiate_Build)
        Me.Controls.Add(Me.TeamRed_List)
        Me.Controls.Add(Me.TeamBlue_List)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.InsertPokemon)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Pokemon_Name)
        Me.Name = "BattleSetup"
        Me.Text = "BattleSetup"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Pokemon_Name As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents InsertPokemon As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TeamBlue_List As System.Windows.Forms.ListView
    Friend WithEvents Pokemon As System.Windows.Forms.ColumnHeader
    Friend WithEvents HP As System.Windows.Forms.ColumnHeader
    Friend WithEvents ATK As System.Windows.Forms.ColumnHeader
    Friend WithEvents DEF As System.Windows.Forms.ColumnHeader
    Friend WithEvents SpATK As System.Windows.Forms.ColumnHeader
    Friend WithEvents SpDEF As System.Windows.Forms.ColumnHeader
    Friend WithEvents Speed As System.Windows.Forms.ColumnHeader
    Friend WithEvents Ability As System.Windows.Forms.ColumnHeader
    Friend WithEvents TeamRed_List As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Worker_FetchMove As System.ComponentModel.BackgroundWorker
    Friend WithEvents Initiate_Build As System.Windows.Forms.Button
    Friend WithEvents Initiate_movebuild As System.Windows.Forms.Button
End Class
