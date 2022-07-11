using Spectre.Console;
namespace GameboyColourDecolouriser.Models;

public record class SpectreTasks(
ProgressTask decolourStageOne,
ProgressTask decolourStageTwo,
ProgressTask decolourStageThree,
ProgressTask decolourStageFour,
ProgressTask generatingFinalImage);