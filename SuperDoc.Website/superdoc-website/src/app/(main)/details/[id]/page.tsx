import { List, ListItem } from "@/common/list/list"
import { ListLayout } from "@/common/list-layout/list-layout"
import { SplitView } from "@/common/split-view/split-view"
import { PdfViewer } from "@/common/pdf-viewer/pdf-viewer"

export default function Details({ params }: any) {

	const list = (
		<SplitView
			left={(
				<div className="h-full">
					<List
						horizontalGridline="enabled"
					>
						{invoices.map((v) => (
							<li key={v.id} className="py-4">
								<ListItem>
									<div className="flex flex-col">
										{v.id}
									</div>
								</ListItem>
							</li>
						))}
					</List>
				</div>
			)}
			right={(<PdfViewer url=""></PdfViewer>)}
		/>
	)


	return (
		<ListLayout
			toolbar={(<h1 className="font-semibold text-xl">Dokumenter</h1>)}
			list={list}
		/>
	)
}


const invoices = [
	{
		id: 1,
		invoice: "INV001",
		paymentStatus: "Paid",
		totalAmount: "$250.00",
		paymentMethod: "Credit Card",
	},
	{
		id: 2,
		invoice: "INV002",
		paymentStatus: "Pending",
		totalAmount: "$150.00",
		paymentMethod: "PayPal",
	},
	{
		id: 3,
		invoice: "INV003",
		paymentStatus: "Unpaid",
		totalAmount: "$350.00",
		paymentMethod: "Bank Transfer",
	},
	{
		id: 4,
		invoice: "INV004",
		paymentStatus: "Paid",
		totalAmount: "$450.00",
		paymentMethod: "Credit Card",
	},
	{
		id: 5,
		invoice: "INV005",
		paymentStatus: "Paid",
		totalAmount: "$550.00",
		paymentMethod: "PayPal",
	},
	{
		id: 6,
		invoice: "INV006",
		paymentStatus: "Pending",
		totalAmount: "$200.00",
		paymentMethod: "Bank Transfer",
	},
	{
		id: 7,
		invoice: "INV007",
		paymentStatus: "Unpaid",
		totalAmount: "$300.00",
		paymentMethod: "Credit Card",
	},
]