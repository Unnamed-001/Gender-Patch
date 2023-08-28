using HarmonyLib;
using RimWorld;
using Verse;

namespace GenderPatch
{
    // Esta clase se encarga de inicializar el mod y aplicar los parches de Harmony
    public class GenderPatchMod : Mod
    {
        public GenderPatchMod(ModContentPack content) : base(content)
        {
            // Creamos una instancia de Harmony con un identificador único para nuestro mod
            var harmony = new Harmony("Unnamed.GenderPatch");

            // Aplicamos todos los parches definidos en este ensamblado
            harmony.PatchAll();
        }
    }

    // Esta clase se encarga de definir una nueva parte del cuerpo llamada genderChanger
    // Esta parte del cuerpo se puede implantar en el torso de un colono y le permite cambiar su género
    public class BodyPartDef_GenderChanger : BodyPartDef
    {
        // Este método se llama cuando se implanta la parte del cuerpo en un colono
        public override void PostImplant(Pawn pawn, BodyPartRecord part)
        {
            base.PostImplant(pawn, part);

            // Cambiamos el género del colono al opuesto al que tenía
            if (pawn.gender == Gender.Male)
            {
                pawn.gender = Gender.Female;
            }
            else if (pawn.gender == Gender.Female)
            {
                pawn.gender = Gender.Male;
            }

            // Mostramos un mensaje en la interfaz informando del cambio de género
            Messages.Message("GenderPatch.GenderChanged".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent);
        }
    }

    // Esta clase se encarga de definir un nuevo tipo de implante llamado genderChanger
    // Este implante se puede fabricar en una mesa de prótesis y requiere una investigación previa
    public class RecipeDef_GenderChanger : RecipeDef
    {
        // Este método se llama cuando se aplica el implante a un colono
        public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
        {
            base.ApplyOnPawn(pawn, part, billDoer, ingredients, bill);

            // Añadimos la parte del cuerpo genderChanger al torso del colono
            pawn.health.AddHediff(HediffDef.Named("GenderChanger"), part);

            // Mostramos un mensaje en la interfaz informando de la operación
            Messages.Message("GenderPatch.ImplantApplied".Translate(billDoer.Named("SURGEON"), pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent);
        }
    }
}
