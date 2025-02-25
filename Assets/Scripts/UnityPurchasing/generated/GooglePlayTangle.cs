// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("drReugXYPHEp9W1vmZY43WLIZ89i4e/g0GLh6uJi4eHgQdNp6+7VWY+M63tuewh9HuZTj3IVu5q3hpkw0GLhwtDt5unKZqhmF+3h4eHl4OP/D8f4QJodq+BuRnyFeuPTbiCNhC1R3x5aJxrYmJBy4szV+nJG8FPlBWrhWMBRHN7B9yUTabSY5bBB5gIqIcx5Y0eYqB9th+MfzQcJiXbGWiMQCDKROcL+4pcn4DLxblI7eh0qmkSDAwzuAIzhCh5LXTzrcSmevdWxvqVSnt6AlHBTKIwslkMpQUiEIrTMv5Yi5wLZTQ/61zsirZ1yE3OtjBh1j0/nO6pNFE609OIxJPLmE/RrFGLxQLKq6sdTB55LBdIS3QP1BeFdaVgN285JLeLj4eDh");
        private static int[] order = new int[] { 4,1,10,4,10,6,11,8,10,13,11,13,13,13,14 };
        private static int key = 224;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
