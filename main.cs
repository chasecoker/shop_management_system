using System;
using System.Collections.Generic;
using System.IO;

class MainClass {
  public static List<string> itemDescription = new List<string>();
  public static List<double> itemPrice = new List<double>();
  public static List<int> itemQoh = new List<int>();

  public static List<string> salesCustomerName = new List<string>();
  public static List<string> salesItemDescription = new List<string>();
  public static List<double> salesItemPrice = new List<double>();
  public static List<int> salesQuantity = new List<int>();

  public static List<string> tempSalesCustomerName = new List<string>();
  public static List<string> tempSalesItemDescription = new List<string>();
  public static List<double> tempSalesItemPrice = new List<double>();
  public static List<int> tempSalesQuantity = new List<int>();

  public static List<string> historyNamesList = new List<string>(); 


  public static void Main (string[] args) {
    LoadData();
    bool keepGoing = true;
    while (keepGoing) {
      string option = MainUserChoice().ToUpper();
      switch (option) {
        case "1" :
          Console.WriteLine("What is your name?");
          string shopperName = Console.ReadLine();
          ShoppingMenu(shopperName);
          break;
        case "2" :
          ManagementMenu();
          break;
        case "Q" :
          keepGoing = false;
          break;
        default :
          Console.WriteLine("That is not one of the choices");
          break;      
      }
    }

    SaveData();
    Console.WriteLine ("Thank you for shopping");
  }
//MENUS--------------------------------------------------------------
  public static void ShoppingMenu(string shopperName){ //"GoShopping"
    // Console.WriteLine("What is your name?");
    // string shopperName = Console.ReadLine(); //validate this input 
    bool keepGoing = true;
    bool keepGoing2 = true;
    while (keepGoing) {
      string option = ShopUserChoice().ToUpper();
      switch (option) {
        case "1" :
          int shopItemIdx = Convert.ToInt32(GetShoppingOption())-1;
          if (shopItemIdx != -1) {
          AddItemToCart(shopperName,shopItemIdx);
          }
          break;
        case "Q" :
          UserCheckOut(shopperName);
          keepGoing = false;
          break;
        default :
          Console.WriteLine("That is not one of the choices");
          break;      
      }
    }
    
  }

  public static void ManagementMenu(){
    bool keepGoing = true;
    while (keepGoing) {
      string option = MgmtUserChoice().ToUpper();
      switch (option) {
        case "1" :
          PrintSalesHistory();
          break;
        case "2" :
          ListAllItems();
          break;
        case "3" :
          AddItemToStore();
          break;
        case "4" :
          ChangeQoh();
          break;
        case "5" :
          ChangePrice();
          break;
        case "Q" :
          keepGoing = false;
          break;
        default :
          Console.WriteLine("That is not one of the choices");
          break;      
      }
    }
    
  }

//MENU OPTIONS-------------------------------------------------------
  public static string MainUserChoice() {
    Console.WriteLine();
    Console.WriteLine("========== Main Menu ==========");
    Console.WriteLine("1. Go Shopping");
    Console.WriteLine("2. Management menu");
    Console.WriteLine("Q. Quit the program");
    Console.WriteLine();      
    Console.WriteLine("Please enter your choice:");
    Console.WriteLine();
    return Console.ReadLine();
  }

  public static string ShopUserChoice() {
    Console.WriteLine();
    Console.WriteLine("========== Shopping Menu ==========");
    Console.WriteLine("1. Choose an item");
    Console.WriteLine("Q. Check out");
    Console.WriteLine();      
    Console.WriteLine("Please enter your choice:");
    Console.WriteLine();
    return Console.ReadLine();
  }

  public static string MgmtUserChoice() {
    Console.WriteLine();
    Console.WriteLine("========== Management Menu ==========");
    Console.WriteLine("1. Browse sales receipts");
    Console.WriteLine("2. View all items");
    Console.WriteLine("3. Add an item");
    Console.WriteLine("4. Change Quantity On Hand");
    Console.WriteLine("5. Change Item Price");
    Console.WriteLine("Q. Return to Main Menu");
    Console.WriteLine();      
    Console.WriteLine("Please enter your choice:");
    Console.WriteLine();
    return Console.ReadLine();
  }
//UNIVERSAL MODULES---------------------------------------------------
  public static int GetItemNumber() {
      ListAllItems();
      Console.WriteLine();
      Console.WriteLine("Please choose an item number (0 to pick none):");
      int itemNumberIndex = Convert.ToInt32(Console.ReadLine());
      if (itemNumberIndex == 0){
        ManagementMenu();  
      }
      return itemNumberIndex;
  }  
  public static void ListAllItems() {
    Console.WriteLine("=========== All Items =============");
    for (int idx=0; idx < itemDescription.Count; idx++) {
      int grammerCheck = itemQoh[idx];
      if (grammerCheck == 1) {
        Console.WriteLine("{3}. {0} costs {1:C}. There is {2} availible", itemDescription[idx], itemPrice[idx], itemQoh[idx], idx+1);
      } else {
        Console.WriteLine("{3}. {0} costs {1:C}. There are {2} availible", itemDescription[idx], itemPrice[idx], itemQoh[idx], idx+1);
        }
      }
    Console.WriteLine("======================================");
  }  

//SHOP MENU MODULES---------------------------------------------------
  public static string GetShoppingOption() {
    bool keepGoing = true;
    string itemOption = "";
    ListAllItems(); 
    Console.WriteLine();
    Console.WriteLine("Please choose an item number (0 to pick none):");
    while (keepGoing) {
      try {
        itemOption = Console.ReadLine();
        int optionInt = Convert.ToInt32(itemOption);
        if (optionInt >= 0 && optionInt <= itemDescription.Count) {
        keepGoing = false;
        }
      } catch (Exception e) {
        Console.WriteLine("Invalid Entry");
      }  
    }
    return itemOption;
  }
  public static void AddItemToCart(string name, int item) {
      bool keepGoing = true;
      while (keepGoing) {
        Console.WriteLine();
        Console.WriteLine("========== Purchasing {0} ==========", itemDescription[item]);
        Console.WriteLine("There are {0} availible for {1:C} each.",itemQoh[item], itemPrice[item]);
        Console.WriteLine("How many would you like to purchase?");
        int requestQoh = Convert.ToInt32(Console.ReadLine());
        if (requestQoh > itemQoh[item]) {
          Console.WriteLine("please enter a valid integer between 0 and {0}", itemQoh[item]);
        } else {
          keepGoing = false;
          int qohDifference = itemQoh[item] - requestQoh;
          itemQoh[item] = qohDifference;
          tempSalesCustomerName.Add(name); 
          tempSalesItemDescription.Add(itemDescription[item]);
          tempSalesItemPrice.Add(Convert.ToDouble(itemPrice[item]));
          tempSalesQuantity.Add(Convert.ToInt32(requestQoh));
        }
      }
      
  }
  public static void UserCheckOut(string name) {
    ShoppingReceipt(name);
    for (int idx=0; idx < tempSalesQuantity.Count; idx++) {
      salesCustomerName.Add(tempSalesCustomerName[idx]);
      salesItemDescription.Add(tempSalesItemDescription[idx]);
      salesItemPrice.Add(tempSalesItemPrice[idx]);
      salesQuantity.Add(tempSalesQuantity[idx]);
    } 
    tempSalesCustomerName.Clear();
    tempSalesItemDescription.Clear();
    tempSalesItemPrice.Clear();
    tempSalesQuantity.Clear();
    SaveData();
  }
  public static void ShoppingReceipt(string name) {
    double totalCost=0;
    int totalUnit=0;
    Console.WriteLine("========== All purchases for {0} ==========", name);
    for (int idx=0; idx < tempSalesQuantity.Count; idx++) {
      double receiptTotal = tempSalesQuantity[idx] * tempSalesItemPrice[idx];
      int salesQoh = tempSalesQuantity[idx];
      Console.WriteLine("{0} of {1} at {2:C} each for {3:C} total",tempSalesQuantity[idx] ,tempSalesItemDescription[idx] ,tempSalesItemPrice[idx] ,receiptTotal );
      totalCost+=receiptTotal;
      totalUnit+=salesQoh;
    }
    Console.WriteLine("Total cost is {0:C} for {1} items",totalCost ,totalUnit );
    Console.WriteLine("===========================================");
  }
  
//MGMT MENU MODULES---------------------------------------------------  
  public static void PrintSalesHistory() {
    CustomerChoice();
    int userChoice = Convert.ToInt32(Console.ReadLine());
    string chosenCustomer = historyNamesList[userChoice-1];
    for (int idx=0; idx < salesCustomerName.Count; idx++) {
      if (chosenCustomer == salesCustomerName[idx]) {
        tempSalesCustomerName.Add(salesCustomerName[idx]);
        tempSalesItemDescription.Add(salesItemDescription[idx]);
        tempSalesItemPrice.Add(salesItemPrice[idx]);
        tempSalesQuantity.Add(salesQuantity[idx]);
      }
    }
    double totalCost=0;
    int totalUnit=0; 
    Console.WriteLine();
    Console.WriteLine("======== All purchases for {0} ========",chosenCustomer);
    for (int idx=0; idx < tempSalesCustomerName.Count; idx++) {
      double transactionTotal = tempSalesItemPrice[idx] * tempSalesQuantity[idx];
      int salesQoh = tempSalesQuantity[idx];
      Console.WriteLine("{0} of {1} at {2:C} each for {3:C} total",tempSalesQuantity[idx],tempSalesItemDescription[idx],tempSalesItemPrice[idx],transactionTotal);
      totalCost+=transactionTotal;
      totalUnit+=salesQoh;
    
    }
    Console.WriteLine();
    Console.WriteLine("Total cost is {0:C} for {1} items",totalCost ,totalUnit );
    Console.WriteLine("===========================================");
    tempSalesCustomerName.Clear();
    tempSalesItemDescription.Clear();
    tempSalesItemPrice.Clear();
    tempSalesQuantity.Clear(); 
  }
  public static void CustomerChoice() {
    historyNamesList.Clear();
    int counter =1;
    Console.WriteLine();
    Console.WriteLine("========== Please choose a customer ==========");
    for (int idx=0; idx < salesCustomerName.Count; idx++) { 
        bool keepGoing = true;
        string customerName = salesCustomerName[idx];
        while (keepGoing) {
          if (historyNamesList.Contains(salesCustomerName[idx])) {
            keepGoing = false;
          } else {
            Console.WriteLine("{0}. {1}",counter ,customerName);
            historyNamesList.Add(customerName);
            counter++;
          }
        }
    }
    Console.WriteLine("Please choose a customer:");
  }
  public static void ChangePrice() { 
    int priceEditIndex = (GetItemNumber()-1);
    Console.WriteLine("Updating price for {0}. Current price is {1:C}",itemDescription[priceEditIndex], itemPrice[priceEditIndex]);
    double newPrice = GetPrice();
    itemPrice[priceEditIndex] = newPrice;
    SaveData();
  }
  public static void ChangeQoh() {
    Console.WriteLine("========== Changing Quantity ==========");
    Console.WriteLine();
    int qohEditIndex = (GetItemNumber()-1);
    Console.WriteLine("Updating quantity for {0}. Current quantity is {1}",itemDescription[qohEditIndex], itemQoh[qohEditIndex]);
    int newQoh = (GetQuantity());
    itemQoh[qohEditIndex] = newQoh;
    SaveData();
  }
  public static void AddItemToStore() {
      Console.WriteLine("Please enter the item description");
      string newItemDescription = Console.ReadLine();
      itemDescription.Add(newItemDescription);
      itemPrice.Add(GetPrice());
      itemQoh.Add(GetQuantity());
      SaveData();
  }
  public static double GetPrice() {
    Console.WriteLine("Please enter the item price (x.xx)");
    double newItemPrice = 0;
    bool keepgoing = true; 
    while (keepgoing) {
      try {
        newItemPrice = Convert.ToDouble(Console.ReadLine());
        keepgoing = false;
      } catch (Exception e) {
        Console.WriteLine("Please enter a valid numeric price.");
      }
    }
    return newItemPrice;
  }
  public static int GetQuantity() {
    Console.WriteLine("Please enter the quanity on hand");
    int newItemQoh = 0;
    bool keepgoing = true; 
    while (keepgoing) {
      try {
        newItemQoh = Convert.ToInt32(Console.ReadLine());
        if (newItemQoh > 0) {
          keepgoing = false;
        }
      } catch (Exception e) {
        Console.WriteLine("Please enter a valid integer for quanitity on hand.");
      }
    }
    return newItemQoh;
  }
      
//LOAD DATA-----------------------------------------------------------
  public static void LoadData(){
    LoadItems();
    LoadSales();
  }

  public static void LoadItems(){
    string filename = "data/items.csv";
    if (File.Exists(filename)) {
      using (var reader = new StreamReader(filename)) {
        reader.ReadLine();
        while (!reader.EndOfStream) {
          string line = reader.ReadLine();
          string[] values = line.Split(",");
          itemDescription.Add(values[0]);
          itemPrice.Add(Convert.ToDouble(values[1]));
          itemQoh.Add(Convert.ToInt32(values[2]));
        }
      }
    }

  }

  public static void LoadSales(){
    string filename = "data/sales.csv";
    if (File.Exists(filename)) {
      using (var reader = new StreamReader(filename)) {
        reader.ReadLine();
        while (!reader.EndOfStream) {
          string line = reader.ReadLine();
          string[] values = line.Split(",");
          salesCustomerName.Add(values[0]);
          salesItemDescription.Add(values[1]);
          salesItemPrice.Add(Convert.ToDouble(values[2]));
          salesQuantity.Add(Convert.ToInt32(values[3]));
        }
      }
    }

  }

//SAVE DATA-----------------------------------------------------------
  public static void SaveData(){
    SaveItems();
    SaveSales();
  }

  public static void SaveItems(){
    string filename = "data/items.csv";
      using (var writer = new StreamWriter(filename)) {
        writer.WriteLine("itemDescription,itemPrice,itemQoh");
        for (int idx=0; idx < itemDescription.Count; idx++) {
          string line = String.Format("{0},{1},{2}", itemDescription[idx], itemPrice[idx], itemQoh[idx]);
          writer.WriteLine(line);
        }
      }  

  }
  
  public static void SaveSales(){ 
    string filename = "data/sales.csv";
      using (var writer = new StreamWriter(filename)) {
        writer.WriteLine("salesCustomerName,salesItemDescription,salesItemPrice,salesQuantity");
        for (int idx=0; idx < salesCustomerName.Count; idx++) {
          string line = String.Format("{0},{1},{2},{3}", salesCustomerName[idx], salesItemDescription[idx], salesItemPrice[idx], salesQuantity[idx]);
          writer.WriteLine(line);
        }
      } 

  }
}