namespace LifeStealClass.Content.Projectiles.Weapon.Sickle
{
    public struct SickleStats
    {
        public float SWINGRANGE;            // Schwungwinkel:               Wie weit die Sense beim normalen Schlag durch den Bogen schwingt.   1f = 360°
        public float SPINRANGE;             // Drehwinkel:                  Wie weit sich die Sense bei Spin Attack dreht.                      1f = 360°

        public float WINDUP;                // Ausholbewegung:              Wie weit die Waffe vor dem Schlag zurückgezogen wird.               0f - 1f
        public float UNWIND;                // Ausschwingen:                Wie lange die Sense nach dem Treffer noch „ausläuft“.               0f - 1f
        public float SPINTIME;              // Drehdauer:                   Wie lange die Spin-Attack dauert.                                   60f = 1 Sekunde

        public float PrepTime;              // Vorbereitungszeit:           Zeit fürs Ausholen vor dem Schlag.                                  60f = 1 Sekunde
        public float ExecTime;              // Angriffszeit:                Zeit für den eigentlichen Schlags.                                  60f = 1 Sekunde
        public float HideTime;              // Rückführzeit:                Wie schnell die Waffe nach dem Schlag verschwindet.                 60f = 1 Sekunde

        public float scale;                 // Skalierung:                  Skaliert die sichtbare Sense.                                       1f = 100% Skalierung (vom Original)
        public float hitboxWidth;           // Trefferbreite:               Dicke deiner Trefferlinie.                                          1 = 1 Pixel (Glaube ich)

        public float rotationOffsetRight;   // Rechts-Rotationsversatz:     Visueller Dreh-Offset für Sprite nach rechts.                       360 = 360°
        public float rotationOffsetLeft;    // Links-Rotationsversatz:      Visueller Dreh-Offset für Sprite nach rechts.                       360 = 360°
    }
}

                                            /*

                                            Schnelle Sichel Beispiel:

                                            SWINGRANGE = 1.2f * MathF.PI
                                            WINDUP = 0.05f
                                            UNWIND = 0.2f
                                            PrepTime = 6f
                                            ExecTime = 5f
                                            HideTime = 6f

                                            Heavy Sichel Beispiel:

                                            SWINGRANGE = 2.0f * MathF.PI
                                            WINDUP = 0.3f
                                            UNWIND = 0.55f
                                            PrepTime = 16f
                                            ExecTime = 12f
                                            HideTime = 14f

                                            Spin Sichel:

                                            SPINRANGE = 5f * MathF.PI
                                            SPINTIME = 4f

                                            */