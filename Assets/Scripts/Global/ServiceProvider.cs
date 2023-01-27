using MyGame;

namespace Global
{
    public class ServiceProvider
    {
        public static IWeaponService FirstLevelWeaponService()
        {
            return new FirstLevelWeaponService();
        }
        public static IWeaponService ThridLevelWeaponService()
        {
            return new ThridLevelWeaponService();
        }
    }
}