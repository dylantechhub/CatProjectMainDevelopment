//remote platform methods

/// <summary>
/// namespace used by the Platform System
/// </summary>
namespace PlatformsData
{


    public static class PlatformRemoteLength
    {

        public static int[] GetMethodRPA(PlatformController PC)
        {
            if (PC.GlobalDebugMode)
            PlatformController.print("[PLATFORMS@REMOTE]: Call Complete!");
            var A1 = 0;
            if (PC.rotatingPlatforms != null)
                A1 = PC.rotatingPlatforms.ToArray().Length;

            var AA = A1;
            int[] AAR = { 0, A1, AA };
            return AAR;
        }

        public static int[] GetMethodMPA(PlatformController PC)
        {
            if (PC.GlobalDebugMode)
            PlatformController.print("[PLATFORMS@REMOTE]: Call Complete!");
            var A2 = 0;
            if (PC.movingPlatforms != null)
                A2 = PC.movingPlatforms.ToArray().Length;

            var AA = A2;
            int[] AAR = { A2, 0, AA };
            return AAR;
        }

        public static int[] GetMethodAll(PlatformController PC)
        {
            if (PC.GlobalDebugMode)
            PlatformController.print("[PLATFORMS@REMOTE]: Call Complete!");
            var A1 = PC.rotatingPlatforms.ToArray().Length;
            var A2 = PC.movingPlatforms.ToArray().Length;
            var AA = A2 + A1;
            int[] AAR = { A2, A1, AA };
            return AAR;
        }

        public static int GetMethodMP(PlatformController PC)
        {
            if (PC.GlobalDebugMode)
            PlatformController.print("[PLATFORMS@REMOTE]: Call Complete!");
            if (PC.movingPlatforms == null)
            {
                return 0;
            }
            else
            {
                var AA = PC.movingPlatforms.ToArray().Length;
                return AA;
            }
        }

        public static int GetMethodRP(PlatformController PC)
        {
            if (PC.GlobalDebugMode)
            PlatformController.print("[PLATFORMS@REMOTE]: Call Complete!");
            if (PC.rotatingPlatforms == null)
            {
                return 0;
            }
            else
            {
                var AA = PC.rotatingPlatforms.ToArray().Length;
                return AA;
            }
        }


    }

    public static class SetRotationSpeedRemote
    { 
        public static void SetGlobal(PlatformController PC, float speed)
        {
            if (PC.GlobalDebugMode)
            PlatformController.print("[PLATFORMS@REMOTE]: Call Complete!");
            foreach (var p in PC.rotatingPlatforms)
            {
                p.LocalRotationSpeed = speed;
            }
        }

        public static void SetLocal(PlatformController PC, float speed, string ID)
        {
            if (PC.GlobalDebugMode)
            PlatformController.print("[PLATFORMS@REMOTE]: Call Complete!");
            foreach (var p in PC.rotatingPlatforms)
            {
                if (p.ID == ID)
                {
                    p.LocalRotationSpeed = speed;
                }
            }
        }

        public static float GetLocal(PlatformController PC, string ID)
        {
            if (PC.GlobalDebugMode)
            PlatformController.print("[PLATFORMS@REMOTE]: Call Complete!");
                foreach (var p in PC.rotatingPlatforms)
                {
                    if (p.ID == ID)
                    {
                        return p.LocalRotationSpeed;
                    } 
                }
                return 0;
        }
    }

        public static class SetMovementSpeedRemote
    { 
        public static void SetGlobal(PlatformController PC, float speed)
        {
            if (PC.GlobalDebugMode)
            PlatformController.print("[PLATFORMS@REMOTE]: Call Complete!");
            foreach (var p in PC.movingPlatforms)
            {
                p.LocalPlatformMovingSpeed = speed;
            }
        }

        public static void SetLocal(PlatformController PC, float speed, string ID)
        {
            if (PC.GlobalDebugMode)
            PlatformController.print("[PLATFORMS@REMOTE]: Call Complete!");
            foreach (var p in PC.movingPlatforms)
            {
                if (p.ID == ID)
                {
                    p.LocalPlatformMovingSpeed = speed;
                }
            }
        }

        public static float GetLocal(PlatformController PC, string ID)
        {
            if (PC.GlobalDebugMode)
            PlatformController.print("[PLATFORMS@REMOTE]: Call Complete!");
                foreach (var p in PC.movingPlatforms)
                {
                    if (p.ID == ID)
                    {
                        return p.LocalPlatformMovingSpeed;
                    } 
                }
                return 0;
        }
    }



    public static class PlatformControllerScript
    {
        public static PlatformController Script { get; set; }
    }

}
