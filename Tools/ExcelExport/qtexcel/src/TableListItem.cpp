#include "TableListItem.hxx"
#include <QtWidgets/QWidget>
#include "ui_TableListItem.h"

TableListItem::TableListItem(QWidget *parent)
	: QWidget(parent), m_ui(new Ui::TableListItem)
{
	m_ui->setupUi(this);
}