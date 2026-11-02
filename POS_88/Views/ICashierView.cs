using System;
using System.Collections.Generic;

namespace POS_88.Views
{
    /// <summary>Step 1 - the contract between the cashier screen and its presenter.
    /// The presenter only ever talks to this interface, never to WinForms controls.</summary>
    public interface ICashierView
    {
        // ---- state the presenter reads / writes ----
        string UserName { get; }
        string SearchText { get; set; }
        SearchMode SearchMode { get; set; }
        /// <summary>Index of the highlighted cart row, or -1 when nothing is selected.</summary>
        int SelectedLineIndex { get; }

        // ---- user intents (raised by the view, handled by the presenter) ----
        event EventHandler SearchTextChanged;              // already debounced by the view
        event EventHandler SearchSubmitted;                // Enter in the search box
        event EventHandler<string> SearchResultChosen;     // item code picked from the result list
        event EventHandler<string> PriceTypeSelected;      // RETAIL / WHOLESALE / PROMO
        event EventHandler TogglePriceTypeRequested;       // Ctrl+D
        event EventHandler ToggleSearchModeRequested;      // Ctrl+S
        event EventHandler EditItemRequested;              // F1
        event EventHandler RemoveRequested;                // F2 / Delete
        event EventHandler DiscountRequested;              // F3
        event EventHandler QuantityRequested;              // F6
        event EventHandler NewSaleRequested;               // F7
        event EventHandler TenderRequested;                // F12
        event EventHandler ReturnRequested;                // Ctrl+5
        event EventHandler<CashierDialog> DialogRequested; // Z-reading, recent sales, cash count...

        // ---- rendering ----
        void BindCart(IList<CartLine> lines, int selectIndex);
        void ShowTotals(CartTotals totals);
        void ShowPaymentSummary(decimal cashTendered, decimal change, decimal redeemAmount);
        void ShowSearchResults(IList<SearchResultItem> items, bool visible);
        void SetPriceType(string priceType);
        void SetEntryEnabled(bool enabled);
        void FocusSearch();
        void ClearSearch();

        // ---- messages & dialogs ----
        void ShowMessage(string text, string caption = "");
        bool Confirm(string text);
        void ShowItemNotFound(string code);
        void OpenDialog(CashierDialog dialog, int receiptNo);
        void ShowDiscountDialog(string itemCode, string priceType);
        void ShowQuantityDialog(CartLine line);
        void ShowVoidItemDialog(CartLine line);
        void ShowEditItemDialog(string itemCode, bool isAdmin);
        void ShowPaymentDialog(decimal amountDue);
        void ShowReceiptDialog(PaymentModel payment);
    }

    /// <summary>Customer-facing pole display + secondary screen, kept apart from the main view.</summary>
    public interface ICustomerFacingDisplay
    {
        void ConfigurePole(bool enabled, string port, string defaultTop, string defaultBottom);
        void LineAdded(CartLine line);
        void LineUpdated(CartLine line);
        void LineRemoved(string itemCode, string priceType);
        void ShowTotals(CartTotals totals, bool writePole);
        void ShowPayment(decimal paid, decimal change);
        void Reset();
    }

}