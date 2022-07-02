using Spectre.Console;
namespace GameboyColourDecolouriser;

public record class SpectreTasks(
ProgressTask decolourStageOne,
ProgressTask decolourStageTwo,
ProgressTask decolourStageThree,
ProgressTask decolourStageFour,
ProgressTask generatingFinalImage);