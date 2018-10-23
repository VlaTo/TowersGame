namespace LibraProgramming.Windows.Games.Engine.Reactive.Extensions
{
    public static class BlueprintExtension
    {
        public static void ApplyToAll(this IBlueprint blueprint, params IEntity[] entities)
        {
            foreach (var entity in entities)
            {
                blueprint.Apply(entity);
            }
        }
    }
}