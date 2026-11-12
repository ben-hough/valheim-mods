using UnityEngine;

namespace GlymorsFeasts
{
    // Keep in sync with tools/build_visuals_v116.py
    // UNITY / in-game board space (the FeastPlains typetree mesh).
    // The extracted OBJ was X-flipped vs Unity; we store the board and all
    // poses in Unity coordinates so the offline renderer matches the game.
    // Handle sticks out along -X (x ≈ -1.35 to -0.88). The main deck is the
    // wide rectangle x ≈ [-0.88, +0.88], z ≈ [-0.89, +0.89], centered at x = 0.
    // Three built-in octagonal ramekins sit in the handle / -Z corner.
    internal static class SerpentsHollowLayout
    {
        internal const float DeckY = 0.111f;
        internal const float SmearLift = 0.0626f; // top (cut) face of the halved roll

        // Jam surface just below the ceramic rim (rim Y = 0.2434).
        internal const float DishFillY = 0.232f;

        internal static readonly Vector3 PotPos = new Vector3(0f, 0f, 0f);
        internal static readonly Vector3 PotEuler = new Vector3(0f, -100f, 0f);
        internal static readonly Vector3 PotScale = new Vector3(1f, 1f, 1f);

        internal static readonly Pose FilledBowl = new Pose(new Vector3(0.370f, 0f, 0.540f), new Vector3(0f, -158f, 0f), 1f);
        internal static readonly Pose EmptyBowl = new Pose(new Vector3(0.450f, 0f, -0.480f), new Vector3(0f, 152f, 0f), 1f);
        internal static readonly Pose Lid = new Pose(new Vector3(0.630f, 0f, -0.620f), new Vector3(16f, -128f, 18f), 1f, 0.012f);

        // Built-in FeastPlains ramekins, measured from the typetree board mesh.
        // Y is the jam-fill height (absolute, place with snap: false).
        internal static readonly Pose DishA = new Pose(new Vector3(-0.7112f, DishFillY, -0.7000f), new Vector3(0f, 20f, 0f), 1f);
        internal static readonly Pose DishB = new Pose(new Vector3(-0.7336f, DishFillY, -0.4510f), new Vector3(0f, 40f, 0f), 1f);
        internal static readonly Pose DishC = new Pose(new Vector3(-0.4553f, DishFillY, -0.6789f), new Vector3(0f, 60f, 0f), 1f);

        internal static readonly Pose WholeA = new Pose(new Vector3(0.650f, 0f, -0.020f), new Vector3(0f, -152f, 0f), 1f);
        internal static readonly Pose WholeB = new Pose(new Vector3(0.630f, 0f, 0.140f), new Vector3(14f, 138f, 10f), 0.98f, 0.100f);
        internal static readonly Pose WholeC = new Pose(new Vector3(0.050f, 0f, 0.660f), new Vector3(0f, -116f, 0f), 1.02f);
        internal static readonly Pose WholeD = new Pose(new Vector3(0.570f, 0f, 0.320f), new Vector3(0f, 162f, 0f), 1f);
        internal static readonly Pose WholeE = new Pose(new Vector3(0.610f, 0f, 0.360f), new Vector3(18f, -144f, -8f), 0.97f, 0.098f);
        internal static readonly Pose WholeF = new Pose(new Vector3(0.630f, 0f, -0.220f), new Vector3(8f, 130f, 6f), 1f, 0.018f);

        // Jam halves in a crescent along the handle, clear of the ramekins.
        internal static readonly Pose Slice1 = new Pose(new Vector3(-0.634f, 0f, 0.296f), new Vector3(0f, -152f, 0f), 1f);
        internal static readonly Pose Slice2 = new Pose(new Vector3(-0.677f, 0f, -0.059f), new Vector3(0f, 160f, 0f), 1f, 0.014f);
        internal static readonly Pose Slice3 = new Pose(new Vector3(-0.387f, 0f, 0.619f), new Vector3(0f, -126f, 0f), 1.02f);
        internal static readonly Pose Slice4 = new Pose(new Vector3(-0.668f, 0f, -0.130f), new Vector3(0f, 118f, 0f), 1f);
        internal static readonly Pose Slice5 = new Pose(new Vector3(-0.665f, 0f, 0.141f), new Vector3(6f, -164f, 4f), 0.98f, 0.058f);
        internal static readonly Pose Slice6 = new Pose(new Vector3(-0.550f, 0f, 0.495f), new Vector3(8f, 140f, 6f), 1f, 0.030f);

        internal struct Pose
        {
            internal Vector3 Pos;
            internal Vector3 Euler;
            internal float Scale;
            internal float Lift;

            internal Pose(Vector3 pos, Vector3 euler, float scale, float lift = 0f)
            {
                Pos = pos;
                Euler = euler;
                Scale = scale;
                Lift = lift;
            }
        }
    }
}
