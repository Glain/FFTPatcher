/*
    Copyright 2007, Joe Davidson <joedavidson@gmail.com>

    This file is part of FFTPatcher.

    FFTPatcher is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FFTPatcher is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FFTPatcher.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PatcherLib
{
    using PatcherLib.Datatypes;
    using PatcherLib.Utilities;
    using Paths = ResourcesClass.Paths.PSX;
    using System.Xml;

    public static partial class PSXResources
    {
        private static IDictionary<ShopsFlags, string> readOnlyStoreNames;
        private static XmlDocument statusNamesDoc;


        private static XmlDocument abilitiesDoc;

        private static XmlDocument abilitiesStringsDoc;

        public static IList<string> CharacterSet { get; private set; }

        private static XmlDocument itemsDoc;

        private static XmlDocument itemsStringsDoc;

        private static XmlDocument jobsDoc;

        private static XmlDocument skillSetsDoc;
        private static XmlDocument specialNamesDoc;
        private static XmlDocument spriteSetsDoc;

        private static XmlDocument shopNamesDoc;

        private static XmlDocument eventNamesDoc;
        private static XmlDocument abilityEffectsDoc;
        private static XmlDocument mapNamesDoc;
        private static XmlDocument unitNamesDoc;
        private static XmlDocument spriteFilesDoc;
        private static XmlDocument braveStoryDoc;
        private static XmlDocument propositionsDoc;

        public static FFTFont PSXFont
        {
            get
            {
                return new FFTFont(PatcherLib.PSXResources.Binaries.Font, PatcherLib.PSXResources.Binaries.FontWidths);
            }
        }

        static PSXResources()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            Binaries.Propositions = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.Propositions].AsReadOnly();
            var defaultProps = ResourcesClass.DefaultZipFileContents[ResourcesClass.Paths.PSX.Binaries.Propositions].AsReadOnly();
            if (Binaries.Propositions.Count < defaultProps.Count)
            {
                List<byte> newProps = new List<byte>( defaultProps.Count );
                newProps.AddRange( Binaries.Propositions );
                newProps.AddRange(
                    defaultProps.Sub( Binaries.Propositions.Count ) );
                Binaries.Propositions = newProps.AsReadOnly();
            }
            Binaries.ReactionAbilityEffects = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSP.Binaries.ReactionAbilityEffects].AsReadOnly();
            Binaries.StoreInventories = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.StoreInventories].AsReadOnly();
            Binaries.ENTD1 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.ENTD1].AsReadOnly();
            Binaries.ENTD2 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.ENTD2].AsReadOnly();
            Binaries.ENTD3 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.ENTD3].AsReadOnly();
            Binaries.ENTD4 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.ENTD4].AsReadOnly();
            Binaries.MoveFind = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.MoveFind].AsReadOnly();
            Binaries.Abilities = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.Abilities].AsReadOnly();
            Binaries.AbilityAnimations = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.AbilityAnimations].AsReadOnly();
            Binaries.AbilityEffects = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.AbilityEffects].AsReadOnly();
            Binaries.ItemAbilityEffects = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.ItemAbilityEffects].AsReadOnly();
            Binaries.ActionEvents = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.ActionEvents].AsReadOnly();
            Binaries.Font = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.Font].AsReadOnly();
            Binaries.FontWidths = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.FontWidths].AsReadOnly();
            Binaries.InflictStatuses = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.InflictStatuses].AsReadOnly();
            Binaries.JobLevels = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.JobLevels].AsReadOnly();
            Binaries.JobFormationSprites1 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.JobFormationSprites1].AsReadOnly();
            Binaries.JobFormationSprites2 = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.JobFormationSprites2].AsReadOnly();
            Binaries.Jobs = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.Jobs].AsReadOnly();
            Binaries.MonsterSkills = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.MonsterSkills].AsReadOnly();
            Binaries.OldItemAttributes = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.OldItemAttributes].AsReadOnly();
            Binaries.OldItems = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.OldItems].AsReadOnly();
            Binaries.PoachProbabilities = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.PoachProbabilities].AsReadOnly();
            Binaries.SkillSets = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.SkillSets].AsReadOnly();
            Binaries.StatusAttributes = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.StatusAttributes].AsReadOnly();
            Binaries.SCEAPDAT = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.Binaries.SCEAP];

            propositionsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.PropositionsXML].ToUTF8String().ToXmlDocument();
            eventNamesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.EventNamesXML].ToUTF8String().ToXmlDocument();
            jobsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.JobsXML].ToUTF8String().ToXmlDocument();
            skillSetsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.SkillSetsXML].ToUTF8String().ToXmlDocument();
            specialNamesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.SpecialNamesXML].ToUTF8String().ToXmlDocument();
            spriteSetsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.SpriteSetsXML].ToUTF8String().ToXmlDocument();
            abilitiesStringsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.AbilitiesStringsXML].ToUTF8String().ToXmlDocument();
            abilityEffectsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.AbilityEffectsXML].ToUTF8String().ToXmlDocument();
            itemsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.ItemsXML].ToUTF8String().ToXmlDocument();
            itemsStringsDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.ItemsStringsXML].ToUTF8String().ToXmlDocument();
            shopNamesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.ShopNamesXML].ToUTF8String().ToXmlDocument();
            mapNamesDoc = ResourcesClass.ZipFileContents[Paths.MapNamesXML].ToUTF8String().ToXmlDocument();
            unitNamesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.UnitNamesXML].ToUTF8String().ToXmlDocument();
            spriteFilesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.SpriteFilesXML].ToUTF8String().ToXmlDocument();
            statusNamesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.StatusNamesXML].ToUTF8String().ToXmlDocument();
            abilitiesDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.AbilitiesNamesXML].ToUTF8String().ToXmlDocument();
            braveStoryDoc = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.BraveStoryXML].ToUTF8String().ToXmlDocument();

            dict[ResourcesClass.Paths.PSX.ItemAttributesXML] = ResourcesClass.ZipFileContents[ResourcesClass.Paths.PSX.ItemAttributesXML].ToUTF8String();

            CharacterSet = new ReadOnlyCollection<string>( new string[77000 / 14 / 10 * 4] {
                "0","1","2","3","4","5","6","7","8","9","A","B","C","D","E","F", 
                "G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V", 
                "W","X","Y","Z","a","b","c","d","e","f","g","h","i","j","k","l", 
                "m","n","o","p","q","r","s","t","u","v","w","x","y","z","!","あ", 
                "?","い","+","う","/","え",":","お","か","が","き","ぎ","く","ぐ","け","げ", 
                "こ","ご","さ","ざ","し","じ","す","ず","せ","ぜ","そ","ぞ","た","だ","ち",".", 
                "っ","つ","づ","て","で","と","ど","な","に","ぬ","ね","の","は","ば","ぱ","ひ", 
                "び","ぴ","ふ","ぶ","ぷ","へ","べ","ぺ","ほ","ぼ","ぽ","ま","み","む","め","も", 
                "ゃ","や","ゅ","ゆ","ょ","よ","ら","り","る","れ","ろ","·","わ","(",")","を", 
                "ん","\"","ア","'","イ","ゥ","ウ","ェ","エ","ォ","オ","カ","ガ","キ","ギ","ク", 
                "グ","ケ","ゲ","コ","ゴ","サ","ザ","シ","ジ","ス","ズ","セ","ゼ","ソ","ゾ","タ", 
                "ダ","チ","♪","ッ","ツ","*","テ","デ","ト","ド","ナ","ニ","ヌ","ネ","ノ","ハ", 
                "バ","パ","ヒ","ビ","ピ","フ","ブ","プ","ヘ","ベ","ペ","ホ","ボ","ポ","マ","ミ", 
                "ム","メ","モ","ャ","ヤ","ュ","ユ","ョ","ヨ","ラ","リ","ル","レ","ロ","ヮ","ワ", 
                "☇","*","ヲ","ン","ヴ","ヵ","ヶ","—","「","、","！","⋯",".","-","＋","×", 
                "÷","∩","∪","＝","≠","＞","＜","≧","≦",
                "*",
                "*",
                "{r }",
                "*",
                "*",
                "*",
                "*", 
                "*",
                "*",
                "*",
                "剣","一","乙","七","丁","九","了","憎","人","入","八","刀","力", 
                "十","下","三","上","丈","万","与","久","丸","乞","也","亡","凡","刃","千","飯", 
                "土","士","夕","大","女","子","寸","小","山","川","工","己","干","弓","々","油", 
                "祭","奇","跡","演","不","中","了","五","互","井","介","仇","今","仁","内","元", 
                "公","六","円","冗","凶","切","分","匂","化","巨","匹","午","厄","双","反","友", 
                "太","天","少","幻","引","心","戸","手","支","文","斗","方","日","月","木","欠", 
                "止","比","毛","水","火","爪","父","片","牛","犬","王","健","康","肌","犯","屍", 
                "敗","我","登","録","丘","世","主","以","仕","仙","他","代","付","令","兄","写", 
                "処","出","加","包","北","半","占","去","収","可","功","句","古","号","史","司", 
                "召","台","右","四","囚","圧","冬","外","央","失","奴","尻","左","市","布","平", 
                "幼","広","必","打","払","旧","札","本","末","未","正","民","永","氷","牙","玉", 
                "甘","生","用","田","甲","申","由","白","皮","目","矢","石","示","礼","穴","立", 
                "込","辺","階","激","両","争","交","仮","会","件","全","伝","光","充","先","兆", 
                "共","再","刑","印","危","各","吉","吸","叫","向","合","吐","同","名","因","回", 
                "団","在","地","壮","多","好","字","存","安","守","年","式","当","成","曲","旬", 
                "早","有","次","死","気","汚","汗","灰","糸","羊","羽","耳","肉","自","至","舌", 
                "色","虫","血","行","衣","西","仲","詳","貢","秩","序","尊","認","赤","良","組", 
                "織","家","評","域","串","乱","亜","位","何","作","住","体","低","伯","伴","余", 
                "児","兵","冷","初","判","別","利","助","努","却","即","囲","告","吹","呆","声", 
                "売","妙","妖","完","対","局","尾","巫","希","形","応","忌","志","忘","快","戒", 
                "戻","技","抗","扱","択","投","攻","杉","杖","束","材","来","汲","決","沈","沌", 
                "没","状","狂","男","系","花","見","角","言","足","身","近","防","条","床","縦", 
                "渾","裏","迎","読","並","乳","価","使","具","刻","制","効","取","受","呼","周", 
                "呪","味","命","固","国","官","実","宝","定","岩","府","彼","念","性","怖","所", 
                "担","抵","抱","放","斧","易","昏","昇","明","肩","服","果","東","枚","武","歩", 
                "毒","泥","法","炎","版","物","的","直","知","祈","空","突","者","英","苦","邪", 
                "金","長","門","阿","雨","責","非","青","沼","城","練","唱","独","況","進","港", 
                "町","傷","唯","商","乗","係","信","保","削","前","南","厚","咲","品","型","変", 
                "契","威","姿","専","封","単","巻","度","後","指","待","律","怒","持","映","星", 
                "冑","染","柱","段","泉","海","活","浄","洞","点","牲","狩","界","発","相","盾", 
                "砂","祝","神","紅","約","美","耐","草","計","負","軍","送","退","追","逃","迷", 
                "重","限","面","革","音","風","飛","食","首","香","荒","屋","拾","口","洋","貿", 
                "拠","展","谷","斜","候","借","値","倒","倍","冥","凍","剛","員","臂","埋","害", 
                "容","射","将","島","差","師","帯","座","弱","修","恐","恵","息","恋","扇","速", 
                "振","料","旅","時","書","胸","能","脈","格","核","根","桁","殺","消","浮","流", 
                "涙","烈","特","珠","疾","真","眠","称","秘","粉","級","素","紋","般","財","起", 
                "造","通","酒","配","閃","陥","降","除","馬","骨","高","鬼","竜","破","耗","原", 
                "宅","建","炭","坑","側","兜","動","寄","宿","巣","常","強","張","御","得","悪", 
                "授","接","探","描","教","断","族","望","械","混","済","深","清","猫","猛","率", 
                "球","現","理","産","異","盗","眼","移","章","笛","経","虚","術","袋","訪","転", 
                "部","陸","魚","鳥","黄","黒","険","終","雪","捨","居","雑","紀","都","街","護", 
                "塞","爵","州","肥","偉","備","喚","喰","善","堅","場","堕","奥","寒","属","嵐", 
                "幅","帽","弾","復","悲","換","揮","揺","散","最","晶","晴","替","普","期","極", 
                "森","棒","温","渦","減","湖","湿","焼","煮","然","無","琴","痛","着","短","程", 
                "等","結","絶","統","葬","葉","落","蛮","装","裂","補","詞","象","貯","超","軽", 
                "軸","運","道","遊","開","間","陽","集","雄","雲","項","順","番","沃","閉","庭", 
                "骸","堀","園","築","催","勢","塊","夢","愛","意","慈","戦","数","暗","腹","楽", 
                "殿","漢","源","準","漠","減","溶","照","鍾","福","窟","置","義","聖","解","該", 
                "話","較","鉄","隕","雷","頑","飾","鼓","違","路","執","務","室","侯","謁","宮", 
                "砦","権","塔","逐","像","塵","増","奪","態","構","槍","模","歌","獄","種","竪", 
                "算","精","聞","蓋","製","複","説","豪","踊","適","酸","銀","銃","関","障","魂", 
                "鳴","境","妃","考","器","導","影","撃","敵","標","潜","熱","確","窮","範","線", 
                "編","舞","蔵","衝","誰","調","賛","輝","輪","選","鋭","震","霊","黙","輸","廊", 
                "排","純","彫","髪","壊","操","樹","獣","磨","薄","薬","賢","避","錬","頭","龍", 
                "犠","翼","闇","整","優","壁","機","環","覧","廷","朝","晩","奏","和","駆","飼", 
                "招","捧","誓","眺","瞬","簡","臨","鎧","鎮","鎌","闘","難","類","騎","験","羅", 
                "覇","護","響","魔","病","伏","茂","湾","崖","野","林","峡","帰","昔","墓","遺", 
                "壇","層","噴","煙","積","満","踏","乾","燥","浅","液","澄","灼","粧","敷","詰", 
                "隠","岸","緑","譲","質","採","珍","柔","芸","玩","際","植","沿","汁","畔","委", 
                "節","脂","泌","徹","船","箱","達","管","鍵","貝","含","堆","粒","鉱","符","沢", 
                "岳","針","塗","承","夜","狭","密","徴","灯","警","銅","施","創","枯","幹","渡", 
                "崩","隙","感","漂","鎚","勇","訴","哀","若","賊","老","慕","蛇","背","握","踵", 
                "蜜","剥","底","軟","盤","茎","絡","樽","貴","飲","資","二","始","続","画","個", 
                "庫","設","更","残","店","欲","買","押","姉","嬉","紹","派","遣","要","竹","証", 
                "廃","役","割","鋼","蓮","崇","爆","腰","膚","職","履","燃","静","治","様","禁", 
                "隣","規","融","語","停","量","腕","急","魅","殊","侵","砕","電","覚","表","事", 
                "総","購","新","記","検","索","訓","思","連","情","拡","縮","括","板","妨","越", 
                "及","傾","誉","愚","許","訳","俺","災","討","卑","劣","郎","似","私","恨","君", 
                "貫","臆","逝","故","郷","刹","那","溺","償","供","慢","救","僅","怯","誇","勝", 
                "談","笑","悟","届","裁","貨","鎖","汝","返","蘇","悠","報","豊","嘆","洗","虔", 
                "捕","醜","願","緩","胞","鐘","淵","誘","漆","幾","瞳","車","如","厳","営","継", 
                "栄","巡","紛","婚","姻","奨","励","煽","宣","庇","損","拒","否","渋","陣","枢", 
                "卿","虐","旗","拗","第","額","区","寂","週","惨","劇","促","領","粛","撤","問", 
                "題","策","謀","賞","懸","輩","惑","讐","論","挟","棟","略","梁","墟","繰","焦", 
                "掌","頼","綱","求","駐","留","被","嫁","Ⅵ","稼","請","伺","憤","懣","逆","離", 
                "想","皆","測","業","狙","走","過","夫","頃","驚","愕","才","娘","疲","渇","端", 
                "為","富","裕","縛","殴","咬","援","鈍","芝","叔","荷","玄","添","腐","奈","奮", 
                "抜","遅","繁","博","覆","提","箇","販","委","秒","嫌","閥","宗","暴","穏","均", 
                "把","途","基","育","汎","忠","糧","秀","膠","任","識","学","逐","距","僧","侶", 
                "衝","凌","胴","粘","挙","述","視","諸","疎","答","顔","参","飽","露","孤","袂", 
                "隷","弟","困","挫","折","勉","疑","緒","泣","尽","悔","征","誕","注","肝","棄", 
                "遠","挑","則","例","淡","費","便","煩","馴","頻","習","践","借","虎","僕","親", 
                "歓","傭","謝","暑","納","皇","帝","諜","罪","卒","赦","詮","是","致","罠","携", 
                "監","怠","駄","蓄","釣","祖","縁","雇","褒","牢","喜","抑","翁","傍","賭","誠", 
                "牧","聡","衛","隊","仰","躍","督","赴","彩","細","到","辛","呂","揆","幸","埒", 
                "朽","預","研","究","興","虜","款","冒","涜","脳","誤","克","憶","隅","働","渉", 
                "豚","屈","叩","叶","吃","観","給","偵","察","奉","政","暮","襲","労","休","案", 
                "享","貪","怨","詫","畜","酷","毎","賠","筈","幽","愁","塩","試","図","婆","庁", 
                "麗","垢","績","母","涯","祥","浪","省","推","網","斥","捜","敢","卓","怪","齢", 
                "辱","准","院","盛","席","筋","延","儀","氏","敬","批","恥","党","株","歴","歯", 
                "拉","協","妥","愉","沖","悩","亭","房","浸","頂","恩","舵","狡","猾","妹","這", 
                "阻","昨","春","衆","拝","堪","忍","屁","雫","横","弔","朱","巧","悶","翌","慎", 
                "社","甚","胆","坊","客","沙","汰","幕","柄","懇","腑","就","掲","罰","盟","益", 
                "欺","懐","璧","慮","講","寝","雰","華","曽","樵","科","姦","猟","曜","浜","勃", 
                "航","狐","謎","卵","校","寡","析","盲","峙","嫉","妬","侍","脱","依","企","託", 
                "墜","憂","諾","還","掃","籍","臣","迫","捉","糾","貧","慣","景","査","控","併", 
                "麻","酔","鯨","村","往","侮","偽","徒","脅","茶","痕","斐","凄","摂","課","綻", 
                "昼","贈","勘","縦","柳","猊","迅","滴","拷","傘","改","脆","弁","審","俗","蔑", 
                "采","叱","悼","莫","煉","遮","浴","歪","甦","劫","惰","寞","冠","濯","河","咆", 
                "哮","智","此","呵","妻","釈","踪","辞","冤","朗","誌","典","孔","嬢","列","些", 
                "維","杯","粋","摘","締","閣","掛","紺","舎","衰","墳","扉","肢","陛","喪","絆", 
                "伐","拘","咤","百","挨","拶","奸","弄","拭","措","婦","池","著","触","偶","堂", 
                "掻","刺","暫","郊","旨","肖","胎","慄","献","郵","京","佐","抽","戴","紙","禍", 
                "垂","軌","溝","崎","匡","某","絵","盆","渾","裏","迎","読","並","乳","価","使", 
                "郭","寺","穀","倉","滝","峠","疑","鶏","碑","潰","祷","鼠","瞳","菊","螺","鈿", 
                "詩","拳","波","斬","勧","蔦","塑","稲","縫","捻","嚇","晄","桜","猪","鼻","臭", 
                "菌","囁","瀕","躱","咏","獲","徐","陰","詠","群","暖","硬","貨","棍","伸","透", 
                "噂","儲","獅","飢","副","蜃","楼","壱","弐","砲","鉾","蟹","秤","蠍","羯","瓶", 
                "坂","斡","旋","歳","隻","邦","掘","遭","遇","霧","悦","酬","絨","毯","橋","癒", 
                "髭","矛","絹","鉢","靴","焉","弦","穂","尖","吟","轄","鴎","漁","館","礁","壌", 
                "穫","夏","逸","舶","畏","匠","窓","艇","泊","棺","篭","哨","麦","募","渓","云", 
                "農","拐","呻","姫","恒","据","孵","唾","滑","胃","芽","尋","呑","枝","梟","載", 
                "鏡","彷","仏","須","搭","∞",".","&","%","○","←","→","・",":","(",")", 
                "\"","'","『","』","」","～","/","△","□","?","♥","Ⅰ","Ⅱ","Ⅲ","Ⅳ","Ⅴ", 
                "♈","♉","♊","♋","♌","♍","♎","♏","♐","♑","♒","♓","{Serpentarius}","脚","后","蟄", 
                "税","亮","塀","囮","邸","昴","曹","傑","覗","訂","嘘","瞑","脆","傲","洛","養", 
                "馳","遁","泡","痴","僭","妾","儚","駅","浦","既","腸","刈","姓","凱","喋","伊", 
                "賀","烙","誅","賤","嗅","讀","牡","挿","筆","艘","遥","溢","撹","掴","貶","捏", 
                "貌","騙","膳","暦","湯","免","又","濃","錯","磁","唸","箔","兼","聴","繋","稚", 
                "旺","釘","函","徹","妄","炸","惚","娯","径","披","瀬","潮","雅","窺","贄","裔", 
                "醒","譚","裸","傀","儡","詔","勅","滞","搾","症","睛","旦","忙","眷","抹","{Unknown}", 
                "=","$","¥","{SP}",",",";","'","\"" } );
        }
    }
}