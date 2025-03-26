// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("RqAjk5g0Q9GP9WVnJXcS6wEvUE8UK3ENt2K2ZW/MtjLw5U/pusE12djAJrDQTzeqCooEOpdSGKMF2QrjuPsiBKBMi3SAf++1GT0nBMWfRmWkG1ZCYjQmibFPMF3uDek/Y5zNRsN9H8Y6oyP82SQceJZ1WclwrVkipbqzKb2m6LVT85DSbbwoWa9pGlsl3CYYdw3Vpsw3OmdceFxRj+kymf+/1e0LP63jYCsVusf2rhwkPfSP/3xyfU3/fHd//3x8fZxzKmJBYqOmlRYHBPetbzxcr19JRml2zNiJqO075LdtdK/l6N4I2jVgizBe8iGqTf98X01we3RX+zX7inB8fHx4fX6qPt0i6XfUzm3D2xlyKE2D2QtsfFpora8faT9R6n9+fH18");
        private static int[] order = new int[] { 7,5,2,11,13,6,11,13,12,11,11,12,13,13,14 };
        private static int key = 125;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
