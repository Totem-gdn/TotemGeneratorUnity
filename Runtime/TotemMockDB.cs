using utilities;
using TotemServices;

public class TotemMockDB
{
    public TotemEntitiesDB EntitiesDB = new TotemEntitiesDB();
    public TotemUsersDB UsersDB = new TotemUsersDB();

    public TotemMockDB()
    {
        MockDBsUtil.PopulateEntitiesDB(EntitiesDB);
        MockDBsUtil.PopulateUsersDB(UsersDB);
        MockDBsUtil.PopulateUsersWithEntities(EntitiesDB, UsersDB);
    }
}
