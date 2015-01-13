#include "TableListItem.hxx"
#include <QtWidgets/QWidget>
#include "ui_TableListItem.h"

TableListItem::TableListItem(QWidget *parent)
	: QWidget(parent), m_ui(new Ui::TableListItem)
{
	m_ui->setupUi(this);
	connect(m_ui->checkBox, SIGNAL(clicked()), this, SLOT(onChkClk()));
}

void TableListItem::setTable(Table* table)
{
	m_table = table;
	m_ui->label->setText(m_table->m_strExcelDirAndName.c_str());
}

void TableListItem::onChkClk()
{
	m_table->m_bExportTable = (m_ui->checkBox->checkState() == Qt::PartiallyChecked);
}