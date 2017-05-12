using HydraDoc.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydraDoc
{
    public enum Theme
    {
        [ThemeResource("hd-mariner.css")]
        Blue,
        [ThemeResource("hd-tia-maria.css")]
        Red,
        [ThemeResource("hd-orange-peel.css")]
        Orange,
        [ThemeResource("hd-la-palma.css")]
        Green,
        [ThemeResource("hd-wedgewood.css")]
        Indigo,
        [ThemeResource( "hd-totem-pole.css" )]
        Brick,
        [ThemeResource( "hd-purple-heart.css" )]
        Purple,
        [ThemeResource( "hd-mango-tango.css")]
        Mango,
        [ThemeResource( "hd-cabaret.css" )]
        Cabaret,
        [ThemeResource( "hd-limeade.css" )]
        Limeade,
        [ThemeResource( "hd-azure.css" )]
        Azure,
        [ThemeResource( "hd-plum.css" )]
        Plum,
        [ThemeResource( "hd-jungle-green.css" )]
        JungleGreen,
        [ThemeResource( "hd-sahara.css" )]
        Sahara,
        [ThemeResource( "hd-bourbon.css" )]
        Bourbon,
        [ThemeResource( "hd-sea-green.css" )]
        SeaGreen,
        [ThemeResource( "hd-malachite.css" )]
        NeonGreen,
        [ThemeResource( "hd-pacific-blue.css" )]
        PacificBlue,
        [ThemeResource( "hd-flirt.css" )]
        Flirt,
        [ThemeResource( "hd-scarlet-gum.css" )]
        Eggplant,
    }
}
