using MyGame;

namespace Global
{
    public class ServiceProvider
    {
        public static IWeaponService WeaponService()
        {
            return new WeaponService();
        }
        public static IWeaponService MockWeaponService()
        {
            return new AnotherWeapoenService();
        }
    }
}