namespace CSDB_UtopiaModel.Model;
class Road : Buildable, Navigable
{
        public int maxSpeed;
        public Vehicle? LeftSide;
        public Vehicle? RightSide;
        public Set<Section> sections;
        public Directions direction;
        public void Road(Coords, maxSpeed, Direction);
        public bool IsFree(Direction);
        public EventHandler<DirectionEventArgs>? Freed;
        public override MoveTo();
    };
