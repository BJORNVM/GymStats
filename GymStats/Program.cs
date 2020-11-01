using GymStats.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GymStats
{
    enum Calculation { Sum, Average, Median, Min, Max, Count }
    enum Property { Weight, Intensity, Repetitions, Volume }
    enum Scope { Person, Log, Block, Cycle, Day, Workout, Exercise, Set }

    class Program
    {
        static void Main(string[] args)
        {
            // *** INFO ***
            // Person > Log > Block > Cycle > Day > Workout > Exercise > Set


            // *** SETUP ***
            // Load data from .xlsx (Excel) file
            DataContext ctx = new DataContext(filePath: Path.Combine(Environment.CurrentDirectory, @"Data\DATA.xlsx"));



            // *** SANDBOX ***

            // Om bijvoorbeeld het gemiddeld volume per workout te vinden van een (of meerdere) geselecteerde cycle(s) moet je
            // een lijst hebben van het totaal volume per workout voor elke workout in de geselecteerde cycle(s)
            // bijvoorbeeld [5000, 4300] 
            // (een cycle heeft meestal meer dan 2 workouts maar dit is enkel ter voorbeeld)
            //
            // Met deze lijst bereken je dan het gemiddelde ---> [5000, 4300].Average()
            //
            // Hierin is het totaal volume per workout de som van het volume van elke exercise in de workout
            // bijvoorbeeld [[2500, 2500], [1500, 1500, 1300]]
            //
            // Hierin is de som van het volume van elke exercise de som van het volume van elke set in de exercise
            // bijvoorbeeld [[[1250, 1250], [1500, 1000]], [[500, 500, 500], [750, 750], [900, 400]]]
            //
            // Maar omdat je de som berekent kan je dit vereenvoudigen en rechtstreeks de som van het volume van elke set in de workout nemen
            // bijvoorbeeld [[1250, 1250, 1500, 1000], [500, 500, 500, 750, 750, 900, 400]]
            // 
            // Bewijs waarom je deze vereenvoudiging kan maken:
            // [[2, 2].Sum(), [3, 3].Sum()].Sum() == [2, 2, 3, 3].Sum() == 10



            // Je moet dus alle gerelateerde workouts vinden van de geselecteerde cycle(s)
            // Omdat er geen directe relatie is tussen een cycle en een workout moet je eerst alle gerelateerde days vinden
            int[] cycleIDs = { 1, 2, 3 };

            IEnumerable<int> dayIDs = ctx.Days.Select(day => day.CycleID).Where(cycleID => cycleIDs.Contains(cycleID));
            IEnumerable<int> workoutIDs = ctx.Workouts.Select(workout => workout.DayID).Where(dayID => dayIDs.Contains(dayID));

            // Eens je alle workouts weet, kan je het totaal volume van alle sets berekenen per workout
            double[] workoutVolumes = workoutIDs.Select(workoutID => {

                // Zoek alle gerelateerde sets van deze workout
                // Omdat er geen directe relatie is tussen een workout en een set moet je eerste alle gerelateerde exercises vinden
                IEnumerable<int> exerciseIDs = ctx.Exercises.Select(exercise => exercise.WorkoutID).Where(ID => ID == workoutID);
                IEnumerable<Set> sets = ctx.Sets.Where(set => exerciseIDs.Contains(set.ExerciseID));
                
                // Totaal volume van alle sets
                double[] setVolumes = sets.Select(set => set.Volume).ToArray();
                double sum = setVolumes.Sum();

                return sum;
            }).ToArray();

            // Van deze workout volumes kan je dan het gemiddeld volume per workout berekenen
            double averageVolumePerWorkout = workoutVolumes.Average();

            // Of een andere berekening...
            double totalVolumeOfAllWorkouts = workoutVolumes.Sum();
            double lowestVolumeInAWorkout = workoutVolumes.Min();
            double highestVolumeInAWorkout = workoutVolumes.Max();
            int amountOfWorkouts = workoutVolumes.Count();
            // Voor aantal workouts moet je niet eerst alle sets en volumes berekenen, dit kan veel sneller
            amountOfWorkouts = workoutIDs.Count();




            // Samenvatting

            // Om een statistiek te berekenen heb je een aantal dingen nodig
            // 1. Een of meerdere bronobjecten                      From List<Person>, Person, List<Log>, Log, List<Block>, Block, ...
            // 2. Een berekening                                    calculate Sum, Average, Median, Min, Max, Count
            // 3. Een property                                      of Weight, Intensity, Repetitions, Volume
            // 4. Een niveau op welk je de berekening wilt          of all Persons, Logs, Blocks, ...

            // Afhankelijk van de keuzes is de omschrijving iets anders
            // From Person, calculate the Sum of Repetitions of all Blocks
            // From Person, calculate the Average of Repetitions per Block
            // From Person, calculate the Min of Repetitions in a Block
            // From Person, calculate the Count of Blocks (hier heb je geen property nodig)


            // :(
        }
    }
}