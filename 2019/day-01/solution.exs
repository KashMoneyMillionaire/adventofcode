Code.require_file("utilities.exs", "..")

defmodule Fuel do
  def for_mass(mass) do
    floor(mass / 3) - 2
  end

  def for_mass_of_mass(mass) do
    new_fuel = for_mass(mass)
    if new_fuel <= 0, do: mass, else: mass + for_mass_of_mass(new_fuel)
  end
end

# Part 1
Utilities.read_list_int()
|> Enum.map(&Fuel.for_mass/1)
|> Enum.sum()
|> IO.puts()

# Part 2
Utilities.read_list_int()
|> Enum.map(&Fuel.for_mass/1)
|> Enum.map(&Fuel.for_mass_of_mass/1)
|> Enum.sum()
|> IO.puts()
