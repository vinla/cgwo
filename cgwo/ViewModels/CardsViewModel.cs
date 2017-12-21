using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cgwo.ViewModels
{
    public class CardsViewModel : ModuleViewModel
    {
        public override bool BeforeUnload()
        {
            return true;
        }
    }
}
