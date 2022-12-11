select * from Animal
select * from Zoo
select a.Name from Animal a inner join ZooAnimal za on a.Id = za.AnimalId where za.ZooId = 1